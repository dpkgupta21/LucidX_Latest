using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Widget;
using Android.App;

namespace LucidX.Droid.Source.Picker
{
    public class DatePickerFragment : Android.Support.V4.App.DialogFragment,
                                   DatePickerDialog.IOnDateSetListener
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "DatePickerFragment";

        // Initialize this value to prevent NullReferenceExceptions.
        Action<DateTime> _dateSelectedHandler = delegate { };
        private DateTime _mSelectedDateTime;

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected, DateTime selectedDateTime)
        {
            DatePickerFragment frag = new DatePickerFragment();
            frag._mSelectedDateTime= selectedDateTime;
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            //DateTime currently = DateTime.Now;
            DatePickerDialog dialog = new DatePickerDialog(Activity, Resource.Style.DialogTheme, 
                                                            this,
                                                            _mSelectedDateTime.Year,
                                                            _mSelectedDateTime.Month-1,
                                                            _mSelectedDateTime.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear+1 , dayOfMonth);          
            _dateSelectedHandler(selectedDate);
        }
    }
}