using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Java.Util;

namespace LucidX.Droid.Source.Picker
{
    public class TimePickerFragment : DialogFragment,
                                   TimePickerDialog.IOnTimeSetListener
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "TimePickerFragment";

        // Initialize this value to prevent NullReferenceExceptions.
        Action<TimeSpan> _timeSelectedHandler = delegate { };

        public static TimePickerFragment NewInstance(Action<TimeSpan> onTimeSet)
        {
            TimePickerFragment frag = new TimePickerFragment();
            frag._timeSelectedHandler = onTimeSet;
            return frag;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            Calendar c = Calendar.Instance;
            int hour = c.Get(CalendarField.HourOfDay);
            int minute = c.Get(CalendarField.Minute);
            bool is24HourView = true;
            TimePickerDialog dialog = new TimePickerDialog(Activity, Resource.Style.DialogTheme,
                                                       this,
                                                       hour,
                                                       minute,
                                                       is24HourView);
            return dialog;
        }

     

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            TimeSpan selectedTime = new TimeSpan(hourOfDay, minute, 00);           
            _timeSelectedHandler(selectedTime);
        }
    }
}