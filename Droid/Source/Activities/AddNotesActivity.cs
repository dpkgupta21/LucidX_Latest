
using Android.OS;
using Android.Views;
using LucidX.Droid.Source.SharedPreference;
using LucidX.Droid.Source.Utilities;
using System;
using Android.Support.V7.App;
using LucidX.Droid.Source.Picker;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using LucidX.Droid.Source.CustomSpinner.Model;
using LucidX.Droid.Source.CustomSpinner.Adapter;
using Android.Graphics;
using LucidX.ResponseModels;
using Newtonsoft.Json;
using LucidX.Droid.Source.CustomViews;
using Plugin.Connectivity;
using LucidX.Webservices;
using Android.Text;
using LucidX.RequestModels;
using LucidX.Droid.Source.Global;
using LucidX.Constants;
using System.Threading.Tasks;
using Android.Content;

namespace LucidX.Droid.Source.Activities
{
    [Activity(Label = "LucidX", Icon = "@mipmap/icon", Theme = "@style/AppTheme",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AddNotesActivity : AppCompatActivity
    {

        /// <summary>
        /// The toolbar
        /// </summary>
        private Android.Support.V7.Widget.Toolbar toolbar;
        private Activity mActivity;
        private SharedPreferencesManager mSharedPreferencesManager;

        private EditText txt_notes_header_val;
        private EditText txt_notes_val;
        private TextView txt_calendar_type;
        private RadioGroup radio_group;

        private Spinner spin_account_code;
        private SpinnerItemModel _selectedAccountCodeItem;
        private SpinnerAdapter _accountCodeSpinnerAdapter;
        private List<SpinnerItemModel> _accountCodeSpinnerItemModelList;
        private int _selectedAccountCodeItemPosition;

        private Spinner spin_current_entity;
        private SpinnerItemModel _selectedCurrentEntitysItem;
        private SpinnerAdapter _entitySpinnerAdapter;
        private List<SpinnerItemModel> _entitySpinnerItemModelList;
        private int _selectedCurrentEntitysItemPosition;
        private LoginResponse objLoginResponse;

        private bool isAddNote;
        private CrmNotesResponse noteObj = null;
        private string entityCode;
        private string accountCode;
        private string accountId;
        private string privacyId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.RequestFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_add_notes);

            mActivity = this;

            /// Shared Preference manager
            mSharedPreferencesManager = UtilityDroid.GetInstance().
                       GetSharedPreferenceManagerWithEncriptionEnabled(mActivity.ApplicationContext);
            objLoginResponse = mSharedPreferencesManager.GetLoginResponse();

            isAddNote = Intent.GetBooleanExtra("isAddNote", false);
            if (!isAddNote)
            {
                string noteObjString = Intent.GetStringExtra("noteObj");
                noteObj = JsonConvert.DeserializeObject<CrmNotesResponse>(noteObjString);
                entityCode = Intent.GetStringExtra("entityCode");
                accountCode = Intent.GetStringExtra("accountCode");
                accountId = Intent.GetStringExtra("accountId");
            }

            try
            {
                Init();
                if (!isAddNote)
                {
                    SetNotesDetails();
                }
            }
            catch (Exception e)
            {

            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.delete, menu);


            IMenuItem menuItem = menu.FindItem(Resource.Id.menu_delete);
            if (isAddNote)
            {
                menuItem.SetVisible(false);
            }
            else
            {
                menuItem.SetVisible(true);
            }
            return true;
        }


        /// <summary>
        /// Init this instance.
        /// </summary>
        private void Init()
        {
            // Init toolbar
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_bar);
            SetSupportActionBar(toolbar);

            // Set toolbar title Add or Edit according to condition
            SetToolbarTitle();
            // Set font for title
            ApplyFontForToolbarTitle();

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            spin_current_entity = FindViewById<Spinner>(Resource.Id.spin_current_entity);
            spin_account_code = FindViewById<Spinner>(Resource.Id.spin_account_code);
            txt_notes_val = FindViewById<EditText>(Resource.Id.txt_notes_val);
            txt_notes_header_val = FindViewById<EditText>(Resource.Id.edt_notes_header_val);


            Button btn_save = FindViewById<Button>(Resource.Id.btn_save);
            btn_save.Click += Btn_save_Click;

            Button btn_cancel = FindViewById<Button>(Resource.Id.btn_cancel);
            btn_cancel.Click += Btn_cancel_Click;

            txt_calendar_type = FindViewById<TextView>(Resource.Id.txt_calendar_type);

            radio_group = FindViewById<RadioGroup>(Resource.Id.radio_group);
            ((RadioButton)radio_group.GetChildAt(0)).Checked = true;
            radio_group.CheckedChange += Radio_group_CheckedChange;
            privacyId = "1";
            // Set Current Entity in Spinner
            InitEntitySpinnerValues();

            // Initialize listener for spinner
            InitializeListeners();
        }



        private void SetNotesDetails()
        {
            try
            {
                txt_notes_val.SetText(Html.FromHtml(noteObj.NotesDetail), TextView.BufferType.Spannable);
                txt_notes_header_val.Text = noteObj.NotesSubject;
                if (noteObj.PrivacyId.Equals("1"))
                {
                    ((RadioButton)radio_group.GetChildAt(0)).Checked = true;
                }
                else if (noteObj.PrivacyId.Equals("2"))
                {
                    ((RadioButton)radio_group.GetChildAt(1)).Checked = true;
                }
            }
            catch (Exception)
            {

            }
        }

        private void SetToolbarTitle()
        {
            if (isAddNote)
            {
                SupportActionBar.SetTitle(Resource.String.add_note_title);
            }
            else
            {
                SupportActionBar.SetTitle(Resource.String.edit_note_title);
            }

        }

        public void ApplyFontForToolbarTitle()
        {
            for (int i = 0; i < toolbar.ChildCount; i++)
            {
                View view = toolbar.GetChildAt(i);
                if (view is TextView)
                {
                    TextView tv = (TextView)view;
                    Typeface titleFont = Typeface.
                       CreateFromAsset(mActivity.Assets, "Fonts/century-gothic.ttf");
                    if (tv.Text.Equals(toolbar.Title))
                    {
                        tv.Typeface = titleFont;
                        break;
                    }
                }
            }
        }


        private void Radio_group_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            RadioGroup radioGroup = (RadioGroup)sender;
            int selectedGroupId = radioGroup.CheckedRadioButtonId;
            if (selectedGroupId == Resource.Id.radio_btn_select)
            {
                txt_calendar_type.Visibility = ViewStates.Visible;
            }
            else if (selectedGroupId == Resource.Id.radio_btn_me_only)
            {
                privacyId = "2";
                txt_calendar_type.Visibility = ViewStates.Gone;
            }
            else if (selectedGroupId == Resource.Id.radio_btn_public)
            {
                txt_calendar_type.Visibility = ViewStates.Gone;
                privacyId = "1";
            }
        }

        /// <summary>
        /// Init values for Current Entity spinner
        /// </summary>
        private async void InitEntitySpinnerValues()
        {
            try
            {
                List<EntityCodesResponse> responseList = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    responseList = await WebServiceMethods.GetEntityCode(Convert.ToInt32(objLoginResponse.UserId), objLoginResponse.UserCompCode);

                    CustomProgressDialog.HideProgressDialog();
                }


                _entitySpinnerItemModelList = new List<SpinnerItemModel>();

                for (int i = 0; i < responseList.Count; i++)
                {

                    SpinnerItemModel item = new SpinnerItemModel
                    {
                        Id = (i + 1) + "",
                        TEXT = responseList[i].CompName,
                        EXTRA_TEXT = responseList[i].CompCode,
                        STATE = false
                    };
                    if (entityCode != null && entityCode.Equals(responseList[i].CompCode + ""))
                    {
                        _selectedCurrentEntitysItemPosition = i;
                    }

                    _entitySpinnerItemModelList.Add(item);
                }
                SetEntitySpinnerAdapter();
            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
            }
        }




        /// <summary>
        /// Set Current Entity spinner adapter
        /// </summary>
        private void SetEntitySpinnerAdapter()
        {
            _entitySpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _entitySpinnerItemModelList);
            spin_current_entity.Adapter = _entitySpinnerAdapter;
            spin_current_entity.SetSelection(_selectedCurrentEntitysItemPosition);
        }



        /// <summary>
        /// Init values for Account Code spinner
        /// </summary>
        private async void InitAccountCodeSpinnerValues()
        {
            try
            {
                List<AccountCodesResponse> responseList = null;
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                        mActivity.Resources.GetString(Resource.String.loading));

                    responseList = await WebServiceMethods.GetAccountCodes(_selectedCurrentEntitysItem.EXTRA_TEXT);

                    CustomProgressDialog.HideProgressDialog();
                }

                _accountCodeSpinnerItemModelList = new List<SpinnerItemModel>();

                for (int i = 0; i < responseList.Count; i++)
                {
                    SpinnerItemModel item = new SpinnerItemModel
                    {
                        Id = (i + 1) + "",
                        TEXT = responseList[i].AccountName,
                        EXTRA_TEXT = responseList[i].AccountId,
                        SECOND_EXTRA_TEXT = responseList[i].AccountCode,
                        STATE = false
                    };
                    if (accountCode != null && accountCode.Equals(responseList[i].AccountCode + ""))
                    {
                        _selectedAccountCodeItemPosition = i;
                    }
                    _accountCodeSpinnerItemModelList.Add(item);
                }
                SetAccountCodeSpinnerAdapter();
            }
            catch (Exception e)
            {
                CustomProgressDialog.HideProgressDialog();
            }
        }

        /// <summary>
        /// Set Account Code spinner adapter
        /// </summary>
        private void SetAccountCodeSpinnerAdapter()
        {
            _accountCodeSpinnerAdapter = new SpinnerAdapter(mActivity, Resource.Layout.spinner_row_item_lay,
                   _accountCodeSpinnerItemModelList);
            spin_account_code.Adapter = _accountCodeSpinnerAdapter;
            spin_account_code.SetSelection(_selectedAccountCodeItemPosition);
        }



        private void InitializeListeners()
        {
            // Current Entity Spinner
            spin_current_entity.ItemSelected += (sender, args) =>
            {
                _selectedCurrentEntitysItem = _entitySpinnerItemModelList[args.Position];

                _entitySpinnerItemModelList[args.Position].STATE = true;
                // update spinner item list state
                for (int i = 0; i < _entitySpinnerItemModelList.Count; i++)
                {
                    if (i == args.Position)
                    {
                        _entitySpinnerItemModelList[i].STATE = true;
                    }
                    else
                    {
                        _entitySpinnerItemModelList[i].STATE = false;
                    }
                }
                _entitySpinnerAdapter.NotifyDataSetChanged();

                InitAccountCodeSpinnerValues();
            };


            // Account Code Spinner
            spin_account_code.ItemSelected += (sender, args) =>
            {
                _selectedAccountCodeItem = _accountCodeSpinnerItemModelList[args.Position];
                _accountCodeSpinnerItemModelList[args.Position].STATE = true;
                // update spinner item list state
                for (int i = 0; i < _accountCodeSpinnerItemModelList.Count; i++)
                {
                    if (i == args.Position)
                    {
                        _accountCodeSpinnerItemModelList[i].STATE = true;
                    }
                    else
                    {
                        _accountCodeSpinnerItemModelList[i].STATE = false;
                    }
                }
                _accountCodeSpinnerAdapter.NotifyDataSetChanged();
            };
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            CallBackScreen();
        }

        private async void Btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        CustomProgressDialog.ShowProgDialog(mActivity,
                            GetString(Resource.String.saving_message));


                        AddNotesAPIParams addNotesParam = new AddNotesAPIParams
                        {
                            entityCode = _selectedCurrentEntitysItem.EXTRA_TEXT,
                            accountCode = _selectedAccountCodeItem.SECOND_EXTRA_TEXT,
                            notesHeader = txt_notes_header_val.Text,
                            notesDetail = txt_notes_val.Text,
                            notesDetail_Add = "",
                            userId = Convert.ToInt32(mSharedPreferencesManager.GetString(
                                    ConstantsDroid.USER_ID_PREFERENCE, "")),
                            actionTypeId = 3,
                            sendTo = "",
                            privacyId = Convert.ToInt32(privacyId),
                            accountId = Convert.ToInt32(_selectedAccountCodeItem.EXTRA_TEXT),
                            notesId = Convert.ToInt32((!isAddNote) ? (noteObj.NotesId) : "0"),
                            connectionName = WebserviceConstants.CONNECTION_NAME
                        };

                        int notesId = await WebServiceMethods.AddCrmNotes(addNotesParam);
                        CustomProgressDialog.HideProgressDialog();

                        CallBackScreen();
                    }
                    else
                    {
                        UtilityDroid.GetInstance().ShowAlertDialog(mActivity,
                            Resources.GetString(Resource.String.error_alert_title),
                            Resources.GetString(Resource.String.alert_message_no_network_connection),
                            Resources.GetString(Resource.String.alert_cancel_btn), Resources.GetString(Resource.String.alert_ok_btn));

                    }
                }
                else
                {
                    UtilityDroid.GetInstance().ShowAlertDialog(mActivity,
                        Resources.GetString(Resource.String.error_alert_title),
                        Resources.GetString(Resource.String.alert_message_fill_all_Details),
                        Resources.GetString(Resource.String.alert_cancel_btn),
                        Resources.GetString(Resource.String.alert_ok_btn));
                }

            }
            catch (Exception ex)
            {
                CustomProgressDialog.HideProgressDialog();
            }

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    CallBackScreen();
                    break;
                case Resource.Id.menu_delete:
                    ShowAlertDialog(Resources.GetString(Resource.String.error_alert_title),
                        GetString(Resource.String.alert_message_delete_notes_confirmation),
                        GetString(Resource.String.alert_cancel_btn),
                        GetString(Resource.String.alert_ok_btn)
                        );
                    break;

            }
            return true;
        }

        private void CallBackScreen()
        {
            Intent returnIntent = new Intent();
            returnIntent.PutExtra("result", "AddNotes");
            SetResult(Result.Ok, returnIntent);
            Finish();
        }
        private async Task<bool> DeleteCRMNotes()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    CustomProgressDialog.ShowProgDialog(mActivity,
                           GetString(Resource.String.processing_message));
                    bool isDelete = await WebServiceMethods.DeleteCrmNotes(noteObj.NotesId);
                    CustomProgressDialog.HideProgressDialog();
                    return isDelete;
                }

            }
            catch (Exception ex)
            {
                CustomProgressDialog.HideProgressDialog();

            }
            return false;
        }

        /// <summary>
        /// Show Alert Dialog
        /// </summary>
        /// <param name="activity">Activity instance</param>
        /// <param name="title">title of alert dialog</param>
        /// <param name="messsage">message to show alert dialog</param>
        /// <returns>Dialog</returns>
        public Dialog ShowAlertDialog(string title, string messsage,
            string positivebtn, string negativeBtn)
        {
            try
            {

                Dialog dialog = null;
                dialog = new CustomDialog().CreateDialog(mActivity,
                                title,
                                messsage,
                                negativeBtn, positivebtn,
                                   new EventHandler(async delegate (Object o, EventArgs a)
                                   {

                                       if (dialog != null)
                                       {
                                           dialog.Dismiss();
                                           dialog = null;
                                       }
                                       bool isDeleted = await DeleteCRMNotes();
                                       if (isDeleted)
                                       {

                                           CallBackScreen();
                                       }
                                   }),
                                 new EventHandler(delegate (Object o, EventArgs a)
                                 {
                                     if (dialog != null)
                                     {
                                         dialog.Dismiss();
                                         dialog = null;
                                     }
                                 }),
                              true, true);

                dialog.SetCancelable(true);
                dialog.Show();
                return dialog;


            }
            catch (Exception e)
            {
                return null;
            }
        }


        private bool ValidateForm()
        {

            if (string.IsNullOrEmpty(txt_notes_header_val.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(txt_notes_val.Text))
            {
                return false;
            }

            return true;
        }
    }

}

