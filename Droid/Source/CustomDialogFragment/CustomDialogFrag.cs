
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using LucidX.Droid.Source.CustomDialogFragment.Adapter;
using LucidX.Droid.Source.Fragments;
using LucidX.Droid.Source.Models;
using LucidX.ResponseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Activity = Android.App.Activity;
namespace LucidX.Droid.Source.CustomDialogFragment
{

    public class CustomDialogFrag : DialogFragment
    {
        private View mView;
        private Activity mActivity;
        private CheckboxDialogAdapter mAdapter;

        public static CustomDialogFrag NewInstance(string notesType)
        {
            var fragment = new CustomDialogFrag();

            Bundle bundle = new Bundle();
            bundle.PutString("notesTypeObj", notesType);
            fragment.Arguments = bundle;

            return fragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mView = inflater.Inflate(Resource.Layout.dialog_list_fragment, container, false);

            mActivity = Activity;

            Dialog.SetTitle(Resource.String.select_calendar_type);
            Dialog.SetCancelable(false); //dismiss window on touch outside

            return mView;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            Button btn_save = mView.FindViewById<Button>(Resource.Id.btn_save);
            btn_save.Click += Btn_save_Click;

            string notesTypeString = Arguments.GetString("notesTypeObj");
            List<NotesTypeResponse> _notesTypeResponse = JsonConvert.
                DeserializeObject<List<NotesTypeResponse>>(notesTypeString);
            // Set Adapter
            SetAdapter(_notesTypeResponse);
        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            try
            {

                List<NotesTypeResponse> updatedNotesTypeList = mAdapter.notesTypeList;
               
                Fragment target = TargetFragment;
                if (target is CalendarFragment) {                
                    ((CalendarFragment)target).GetNotesTypeResult(updatedNotesTypeList);
                }

                Dismiss();
            }
            catch (Exception ex)
            {

            }
        }

        private void SetAdapter(List<NotesTypeResponse> notesTypeResponse)
        {
            ListView listView = mView.FindViewById<ListView>(Resource.Id.listview);

            mAdapter = new CheckboxDialogAdapter
                (notesTypeResponse, mActivity);
            listView.Adapter = mAdapter;
        }
      
    }
}