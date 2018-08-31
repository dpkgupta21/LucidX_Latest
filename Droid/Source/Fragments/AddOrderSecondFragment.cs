
using Android.OS;
using Android.Views;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Utilities;
using System;
using Android.Support.V4.App;
using Android.Widget;
using LucidX.Droid.Source.CustomDialogFragment;
using LucidX.Droid.Source.Models;
using System.Collections.Generic;
using Android.Content;
using LucidX.Droid.Source.Adapters;
using Android.Graphics;
using Activity = Android.App.Activity;

using LucidX.ResponseModels;
using LucidX.Droid.Source.Activities;
using Plugin.Connectivity;
using LucidX.Droid.Source.CustomViews;
using LucidX.Webservices;
using Newtonsoft.Json;

namespace LucidX.Droid.Source.Fragments
{

    public class AddOrderSecondFragment : Fragment
    {


        private Activity mActivity;
        private SharedPreferencesManager mSharedPreferencesManager;

        private AddOrderAdapter mAdapter;

        private ListView lstView;
        private TextView txt_no_list;
        private View view;

        private bool isEdit;
        private LedgerOrder ledgerOrderObj;
        private List<LedgerOrderItem> ledgerOrderItemLst;

        public static Fragment GetInstance()
        {
            AddOrderSecondFragment fragment = new AddOrderSecondFragment();
            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public LedgerOrder GetLedgerOrderObj()
        {
            return ((AddOrderFirstActivity)mActivity).LedgerOrderObj;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            view = inflater.Inflate(Resource.Layout.activity_add_second_order, container, false);

            mActivity = Activity;
            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);


            isEdit = ((AddOrderFirstActivity)mActivity).isEdit;
            ledgerOrderObj = ((AddOrderFirstActivity)mActivity).LedgerOrderObj;



            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            try
            {
                Init();

                ShowLedgerItems();

            }
            catch (Exception e)
            {

            }
        }


        /// <summary>
        /// Init this instance.
        /// </summary>
        private void Init()
        {
            lstView = view.FindViewById<ListView>(Resource.Id.listview);
            lstView.ItemClick += LstView_ItemClick;

            txt_no_list = view.FindViewById<TextView>(Resource.Id.txt_no_list);
            Button btn_add_order = view.FindViewById<Button>(Resource.Id.btn_add_order);
            btn_add_order.Click += Btn_add_order_Click; ;

            Button btn_next = view.FindViewById<Button>(Resource.Id.btn_next);
            btn_next.Click += Btn_next_Click;
        }

        private void ShowLedgerItems()
        {
            if (ledgerOrderObj.LedgerOrderItems != null && ledgerOrderObj.LedgerOrderItems.Count != 0)
            {
                ledgerOrderItemLst = ledgerOrderObj.LedgerOrderItems;
                InitializeAdapter();
            }
            else
            {
                ledgerOrderItemLst = new List<LedgerOrderItem>();
                ledgerOrderObj.LedgerOrderItems = ledgerOrderItemLst;
            }

        }

        /// <summary>
        /// Handles when list view row click. It opens in view mode only
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int pos = e.Position;

            string ledgerOrderItemString = JsonConvert.SerializeObject(ledgerOrderItemLst[pos]);
            ShowOrderDialogFragment(ledgerOrderItemString, true, pos, false);
        }

        /// <summary>
        /// Handles when list view row edit button click. It opens in edit mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pos"></param>
        private void MAdapter_EditOrderClick(object sender, int pos)
        {
            string ledgerOrderItemString = JsonConvert.SerializeObject(ledgerOrderItemLst[pos]);
            ShowOrderDialogFragment(ledgerOrderItemString, true, pos, true);
        }

        private void Btn_add_order_Click(object sender, EventArgs e)
        {
            //isEditOrderItem is false means new order add or edit
            ShowOrderDialogFragment(null, false, -1, true);
        }

        private void ShowOrderDialogFragment(string ledgerOrderItemString, bool isEditOrderItem,
            int itemPos, bool inEditMode)
        {
            try
            {
                FragmentTransaction fragmentTransaction = FragmentManager.BeginTransaction();

                //remove fragment from backstack if any exists
                Fragment fragmentPrev = FragmentManager.FindFragmentByTag("dialog");
                if (fragmentPrev != null)
                    fragmentTransaction.Remove(fragmentPrev);

                fragmentTransaction.AddToBackStack(null);
                //create and show the dialog
                OrderAddDialogFrag dialogFragment = OrderAddDialogFrag.NewInstance(this, ledgerOrderItemString,
                    isEditOrderItem, itemPos, inEditMode);
                dialogFragment.Show(fragmentTransaction, "dialog");
            }
            catch (Exception ex)
            {

            }
        }

        private void Btn_next_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                ledgerOrderObj.LedgerOrderItems = ledgerOrderItemLst;
                ((AddOrderFirstActivity)mActivity).LedgerOrderObj = ledgerOrderObj;
                DisplayFragment();
            }
            else
            {
                UtilityDroid.GetInstance().ShowAlertDialog(mActivity, Resources.GetString(Resource.String.error_alert_title),
                     Resources.GetString(Resource.String.alert_message_atleast_add_one_order),
                     Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));

            }
        }


        private void DisplayFragment()
        {
            Fragment fragment = AddOrderThirdFragment.GetInstance();
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.frame_container, fragment, fragment.Class.Name);
            ft.AddToBackStack(null);
            ft.Commit();
        }


        public void AddLedgerOrderItem(LedgerOrderItem ledgerOrderItemModel, int pos)
        {
            try
            {
                if (pos == -1)
                {
                    ledgerOrderItemLst.Add(ledgerOrderItemModel);
                    InitializeAdapter();
                }
                else
                {
                    ledgerOrderItemLst[pos] = ledgerOrderItemModel;
                    mAdapter.NotifyDataSetChanged();
                }

            }
            catch (Exception e)
            {

            }
        }


        private void InitializeAdapter()
        {
            try
            {
                if (ledgerOrderItemLst != null && ledgerOrderItemLst.Count != 0)
                {
                    lstView.Visibility = ViewStates.Visible;
                    txt_no_list.Visibility = ViewStates.Gone;

                    mAdapter = new AddOrderAdapter(ledgerOrderItemLst, mActivity);
                    mAdapter.EditOrderClick += MAdapter_EditOrderClick;
                    lstView.Adapter = mAdapter;

                }
                else
                {
                    lstView.Visibility = ViewStates.Gone;
                    txt_no_list.Visibility = ViewStates.Visible;
                }
            }
            catch (Exception e)
            {

            }
        }

       

        private bool ValidateForm()
        {

            if (ledgerOrderItemLst == null || ledgerOrderItemLst.Count == 0)
            {
                return false;
            }


            return true;
        }
    }

}

