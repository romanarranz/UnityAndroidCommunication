package com.github.romanarranz.blockgame;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Configuration;
import android.graphics.PixelFormat;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.Window;
import android.widget.FrameLayout;

import com.unity3d.player.UnityPlayer;

import java.util.Random;

public class UnityPlayerActivity extends Activity
{
    private static final String LOG_TAG = UnityPlayerActivity.class.getSimpleName();

    protected UnityPlayer mUnityPlayer; // don't change the name of this variable; referenced from native code
    private FrameLayout mUnityHolder;
    private Float mPlayerWidth = 0f;
    private boolean mStop = true;
    private Handler mHandler = new Handler();

    private Thread mTickUI = new Thread(new Runnable() {
        @Override
        public void run() {
            float minX = -mPlayerWidth;
            float maxX = mPlayerWidth;

            Random rand = new Random();

            float finalX = rand.nextFloat() * (maxX - minX) + minX;

            mUnityPlayer.UnitySendMessage("Player", "ReceiveMsg", Float.toString(finalX));

            if (!mStop) {
                mHandler.postDelayed(mTickUI, 1000 / 60); // refrescar cada 60fps
            }
        }
    });

    // Setup activity layout
    @Override protected void onCreate (Bundle savedInstanceState)
    {
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        super.onCreate(savedInstanceState);
        setContentView(R.layout.unity_activity);

        getWindow().setFormat(PixelFormat.RGBX_8888); // <--- This makes xperia play happy

        mUnityPlayer = new UnityPlayer(this);
        mUnityHolder = findViewById(R.id.unity_holder);

        FrameLayout.LayoutParams lp = new FrameLayout.LayoutParams(
                FrameLayout.LayoutParams.MATCH_PARENT,
                FrameLayout.LayoutParams.MATCH_PARENT
        );

        mUnityHolder.addView(mUnityPlayer.getView(), 0, lp);

        mPlayerWidth = 5f;

        mUnityPlayer.requestFocus();

        // el inicio del modo random está hardcodeado
        Log.i(LOG_TAG, "###################");
        Log.i(LOG_TAG, UnityPlayerActivity.class.getCanonicalName());
        mStop = false;
        mHandler.post(mTickUI);
    }

    @Override protected void onNewIntent(Intent intent)
    {
        // To support deep linking, we need to make sure that the client can get access to
        // the last sent intent. The clients access this through a JNI api that allows them
        // to get the intent set on launch. To update that after launch we have to manually
        // replace the intent with the one caught here.
        setIntent(intent);
    }

    // Quit Unity
    @Override protected void onDestroy ()
    {
        mUnityPlayer.quit();
        super.onDestroy();
    }

    // Pause Unity
    @Override protected void onPause()
    {
        super.onPause();
        mUnityPlayer.pause();
    }

    // Resume Unity
    @Override protected void onResume()
    {
        super.onResume();
        mUnityPlayer.resume();
    }

    // Low Memory Unity
    @Override public void onLowMemory()
    {
        super.onLowMemory();
        mUnityPlayer.lowMemory();
    }

    // Trim Memory Unity
    @Override public void onTrimMemory(int level)
    {
        super.onTrimMemory(level);
        if (level == TRIM_MEMORY_RUNNING_CRITICAL)
        {
            mUnityPlayer.lowMemory();
        }
    }

    // This ensures the layout will be correct.
    @Override public void onConfigurationChanged(Configuration newConfig)
    {
        super.onConfigurationChanged(newConfig);
        mUnityPlayer.configurationChanged(newConfig);
    }

    // Notify Unity of the focus change.
    @Override public void onWindowFocusChanged(boolean hasFocus)
    {
        super.onWindowFocusChanged(hasFocus);
        mUnityPlayer.windowFocusChanged(hasFocus);
    }

    // For some reason the multiple keyevent type is not supported by the ndk.
    // Force event injection by overriding dispatchKeyEvent().
    @Override public boolean dispatchKeyEvent(KeyEvent event)
    {
        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent(event);
        return super.dispatchKeyEvent(event);
    }

    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    @Override public boolean onKeyUp(int keyCode, KeyEvent event)     { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onKeyDown(int keyCode, KeyEvent event)   { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onTouchEvent(MotionEvent event)          { return mUnityPlayer.injectEvent(event); }
    /*API12*/ public boolean onGenericMotionEvent(MotionEvent event)  { return mUnityPlayer.injectEvent(event); }

    // Se llama en Player.cs Start() para comunicar el width disponible
    public void playerWidth(String strFromUnity) {
        mPlayerWidth = Float.parseFloat(strFromUnity);

        // en cuanto nos llegue el dato comenzar a mover el player
        if (mStop) {
            mStop = false;
            mHandler.post(mTickUI);
        }
    }
}