package com.example.myapplication;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;

import java.io.BufferedWriter;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.Socket;

public class joystick extends AppCompatActivity {
    private PrintWriter mBufferOut;
    private Socket socket;
    private String ip;
    private int port;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Intent intent = getIntent();
        if (intent != null) {
            //get the values from last activity
            String ipGet = intent.getStringExtra("ip");
            String portGet = intent.getStringExtra("port");
            port = Integer.parseInt(portGet);
            ip = ipGet;
        } else {
            ip = "10.0.2.2";
            port = 1234;

        }
        //connect to the server according to the ip and port
        connectServer();
        JoystickBoard joystickBoard = new JoystickBoard(this, mBufferOut);
        setContentView(joystickBoard);
    }

    public void connectServer() {
        Runnable runnable = new Runnable() {
            @Override
            public void run() {
                try {
                    //here you must put your computer's IP address.
                    InetAddress serverAddr = InetAddress.getByName(ip);

                    //create a socket to make the connection with the server
                    socket = new Socket(serverAddr, port);

                    try {
                        //sends the message to the server
                        mBufferOut = new PrintWriter(new BufferedWriter(new OutputStreamWriter(socket.getOutputStream())), true);

                    } catch (Exception e) {
                        Log.e("TCP", "Error: can't create writer", e);
                        //if it failed opening the writer
                        socket.close();
                    }
                } catch (Exception e) {
                    Log.e("TCP", "Error: can't open socket", e);
                }
            }
        };
        //run the thread
        Thread thread = new Thread(runnable);
        thread.start();
    }


    @Override
    protected void onDestroy() {
        disconnectServer();
        super.onDestroy();
    }


    public void disconnectServer() {
        try {
            mBufferOut.close();
        } catch (Exception e) {
            Log.e("Exception", "Error: Can't close mBufferOut", e);
        }
        if (socket.isClosed()) {
            try {
                socket.close();
            } catch (Exception e) {
                Log.e("Exception", "Error: Can't close socket", e);
            }
        }
    }
}