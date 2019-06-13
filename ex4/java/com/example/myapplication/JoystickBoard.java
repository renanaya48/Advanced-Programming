package com.example.myapplication;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Point;
import android.graphics.RectF;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;

import java.io.PrintWriter;
import java.util.concurrent.Semaphore;

public class JoystickBoard extends View {
    private Paint outOval;
    private Paint inArc;
    private int outWidth, outHeight;
    private int xPosCenter, yPosCenter;
    private int xCurrentPos, yCurrentPos;
    private PrintWriter mBufferOut;
    private static Semaphore mutex = new Semaphore(1);
    private String ip;
    private String port;
    private boolean isTouch = false;
    //Drawing drawing;

    public static final int r = 100;

    public JoystickBoard(Context context, PrintWriter mBufferOutGet) {
        super(context);

        outOval = new Paint(Paint.ANTI_ALIAS_FLAG);
        outOval.setColor(Color.GREEN);
        outOval.setStyle(Paint.Style.FILL);
        inArc = new Paint(Paint.ANTI_ALIAS_FLAG);
        inArc.setColor(Color.BLUE);
        inArc.setStyle(Paint.Style.FILL);
        ip = ip;
        port = port;
        mBufferOut = mBufferOutGet;

    }

    protected void onSizeChanged(int w, int h, int oldW, int oldH) {
        super.onSizeChanged(w, h, oldW, oldH);

        int xPad = getPaddingLeft() + getPaddingRight();
        int yPad = getPaddingTop() + getPaddingBottom();

        outWidth = w - xPad;
        outHeight = h - yPad;

        xPosCenter = outWidth/2;
        yPosCenter = outHeight/2;
        xCurrentPos = xPosCenter;
        yCurrentPos = yPosCenter;

    }

    @Override
    protected void onDraw(Canvas canvas) {
        super.onDraw(canvas);

        RectF oval = new RectF(0, 0, outWidth, outHeight);
        canvas.drawOval(oval, outOval);
        canvas.drawCircle(xCurrentPos, yCurrentPos, 150, inArc);
    }


    @Override
    public boolean onTouchEvent(MotionEvent event){
        int action = event.getAction();
        switch (action){
            //press the button
            case MotionEvent.ACTION_DOWN: {
                float x =  event.getX();
                float y = event.getY();
                //out from screen
                if ((x - r <= 0) || (x + r >= outWidth) || (y - r <= 0) || (y + r >= outHeight)) {
                    isTouch = false;
                    break;
                } else {
                    //update the place of the point
                    xCurrentPos = (int)x;
                    yCurrentPos = (int)y;
                    isTouch = true;
                }
                break;
            }

            //relese
            case MotionEvent.ACTION_UP:{
                float x =  event.getX();
                float y = event.getY();
                //set the values
                setValue(x, y);
                //drag the joystick to the center
                xCurrentPos = xPosCenter;
                yCurrentPos = yPosCenter;
                isTouch = false;
                break;
            }

            //move the joystick
            case MotionEvent.ACTION_MOVE:{
                if(!isTouch)
                    return true;
                float x =  event.getX();
                float y = event.getY();
                //out of screen
                if((x-r<=0)||(x+r>=outWidth)||(y-r<=0)||(y+r>=outHeight)){
                    isTouch = false;
                    break;
                }
                else{
                    //get the current values of the joystick
                    xCurrentPos = (int)x;
                    yCurrentPos = (int)y;
                    isTouch = true;
                    break;
                }
            }

            //cancel
            case MotionEvent.ACTION_CANCEL:{
                //drag to center
                xCurrentPos = xPosCenter;
                yCurrentPos = yPosCenter;
                isTouch = false;
                break;
            }
            default:
                break;

        }
        invalidate();
        return true;
    }

    public void writeMessage(final String message) {
        Runnable runnable = new Runnable() {
            @Override
            public void run() {
                try{
                    //critical section
                    mutex.acquire();

                    //writing the message to the server
                    mBufferOut.println(message);
                    mBufferOut.flush();

                    //end of critical section
                    mutex.release();

                } catch (Exception e){
                    Log.e("TCP", "mutex error");
                }
            }
        };
        //run the thread
        Thread thread = new Thread(runnable);
        thread.start();
    }

    public void setValue(float x, float y){
        float aileron;
        float elevator;
        String msg;

        //set the new value to the aileron
        aileron = (x - xPosCenter) / xPosCenter;

        //make sure it doesn't crooss the borders
        if(aileron>1){
            aileron = 1;
        }
        else if(aileron < -1){
            aileron = -1;
        }
        msg = "set controls/flight/aileron ";
        msg = msg + aileron;
        msg = msg + "/r/n";

        //write the message to the server
        writeMessage(msg);

        //set the new value to the elevator
        elevator = (y - yPosCenter) / yPosCenter;

        //make sure it doesn't crooss the borders
        if(elevator>1){
            elevator = 1;
        }
        else if(elevator < -1){
            elevator = -1;
        }
        msg = "set controls/flight/elevator ";
        msg = msg + elevator;
        msg = msg + "/r/n";
        //write the message to the server
        writeMessage(msg);

    }

}
