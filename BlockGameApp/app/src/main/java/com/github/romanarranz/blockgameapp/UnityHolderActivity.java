package com.github.romanarranz.blockgameapp;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;

import com.github.romanarranz.blockgame.UnityPlayerActivity;

/**
 * Created by romanarranzguerrero on 27/8/17.
 */

public class UnityHolderActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Intent intent = new Intent(this, UnityPlayerActivity.class);
        startActivity(intent);
    }
}
