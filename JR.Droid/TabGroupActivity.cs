using System;
using System.Collections;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace JR.Droid
{
	public class TabGroupActivity : ActivityGroup
	{
		private ArrayList mIdList;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			if (mIdList == null) mIdList = new ArrayList();

		}

		public void FinishFromChild(Activity child)
		{
			LocalActivityManager manager = base.LocalActivityManager;
			int index = mIdList.Count - 1;
			if (index < 1)
			{
				Finish();
				return;
			}
			String lastId = (String)mIdList[index];
			/*
            int i = 0;
            foreach (String temp in mIdList) {
                if (i == index) lastId = temp;
                i++;
            }*/

			manager.DestroyActivity(lastId, true);
			mIdList.RemoveAt(index);
			index--;
			Intent lastIntent = manager.GetActivity(lastId).Intent;
			Window newWindow = manager.StartActivity(lastId, lastIntent);
			SetContentView(newWindow.DecorView);
		}

		public void StartChildActivity(String Id, Intent intent)
		{
			//TODO CONTROLLARE FLAG_ACTIVITY_CLEAR_TOP 67108864 
			Window window = LocalActivityManager.StartActivity(Id, intent.AddFlags(ActivityFlags.ClearTop));
			if (window != null)
			{
				mIdList.Add(Id);
				SetContentView(window.DecorView);
			}
		}		/**
       * The primary purpose is to prevent systems before android.os.Build.VERSION_CODES.ECLAIR
       * from calling their default KeyEvent.KEYCODE_BACK during onKeyDown.
       */
		public bool OnKeyDown(Keycode keyCode, KeyEvent evento) {
			if (keyCode == Keycode.Back) {
				//preventing default implementation previous to android.os.Build.VERSION_CODES.ECLAIR
				OnBackPressed();
				return true;
			}
			return base.OnKeyDown(keyCode, evento);
		}

		/**
       * Overrides the default implementation for KeyEvent.KEYCODE_BACK 
       * so that all systems call onBackPressed().
       */
		public bool onKeyUp(Keycode keyCode, KeyEvent evento) {
			if (keyCode == Keycode.Back) {
				//onBackPressed();
				return true;
			}
			return base.OnKeyUp(keyCode, evento);
		}

		/**
       * If a Child Activity handles KeyEvent.KEYCODE_BACK.
       * Simply override and add this method.
       */  
		public void OnBackPressed  () {
			int length = mIdList.Count;

			if ( length > 1) {
				Activity current = LocalActivityManager.GetActivity((String)mIdList[length-1]);
				if(current!=null)
					current.Finish();
			}  else {

				Finish();
				/*
    	      new AlertDialog.Builder(this).SetMessage("Sei sicuro di voler uscire?").SetPositiveButton("SI", new OnClickListener(){

				    public void onClick(DialogInterface arg0, int arg1) {
					    Finish();
					
				    }
	    		
	    		
	    	    }).SetNegativeButton("No", null).show();*/
				// Results.isFirstRun = true;

				//finish();
			}
		}



	}
}