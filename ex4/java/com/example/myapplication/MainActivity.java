package com.example.myapplication;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }
    public void sendConncet(View view){
        EditText editText = (EditText)findViewById(R.id.ip_insert);
        //send the ip and port from the user
        Intent intent = new Intent(this, joystick.class);
        String ip = editText.getText().toString();
        intent.putExtra("ip", ip);
        EditText editText1 = (EditText)findViewById(R.id.port_insert);
        String port = editText1.getText().toString();
        intent.putExtra("port", port);

        startActivity(intent);


        //textView.setText("Message: " + editText.getText().toString());
    }

}
