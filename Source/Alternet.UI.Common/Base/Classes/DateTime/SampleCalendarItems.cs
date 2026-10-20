using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a set of sample calendar items for demonstration purposes.
    /// </summary>
    public static class SampleCalendarItems
    {
        /// <summary>
        /// Loads sample calendar items into the specified <see cref="CalendarItemSource"/>.
        /// </summary>
        /// <param name="server"></param>
        public static void LoadSampleData(CalendarItemSource server)
        {
            var createNewDemo = server.CreateItem();
            createNewDemo.Start = DateTime.Today.AddDays(0).AddHours(0).AddMinutes(0).AddSeconds(0);
            createNewDemo.End = DateTime.Today.AddDays(2).AddHours(0).AddMinutes(0).AddSeconds(0);
            createNewDemo.Text = "Create New Demo";
            createNewDemo.Description = "The new Scheduler demo.";
            createNewDemo.Location = "work";
            createNewDemo.Category = KnownCalendarItemMarkers.CategoryImportant;
            createNewDemo.Status = KnownCalendarItemMarkers.StatusBusy;
            createNewDemo.AlarmInfo.Enabled = true;
            createNewDemo.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(createNewDemo);

            var gototheBeach = server.CreateItem();
            gototheBeach.Start = DateTime.Today.AddDays(0).AddHours(18).AddMinutes(0).AddSeconds(0);
            gototheBeach.End = DateTime.Today.AddDays(0).AddHours(21).AddMinutes(0).AddSeconds(0);
            gototheBeach.Text = "Go to the Beach";
            gototheBeach.Description = "Try to surf-riding.";
            gototheBeach.Location = "home";
            gototheBeach.Category = KnownCalendarItemMarkers.CategoryPersonal;
            gototheBeach.Status = KnownCalendarItemMarkers.StatusFree;
            gototheBeach.AlarmInfo.Enabled = true;
            gototheBeach.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(gototheBeach);

            var calltofather = server.CreateItem();
            calltofather.Start = DateTime.Today.AddDays(0).AddHours(20).AddMinutes(0).AddSeconds(0);
            calltofather.End = DateTime.Today.AddDays(0).AddHours(20).AddMinutes(30).AddSeconds(0);
            calltofather.Text = "Call to father";
            calltofather.Category = KnownCalendarItemMarkers.CategoryPhoneCall;
            calltofather.Status = KnownCalendarItemMarkers.StatusUnknown;
            calltofather.AlarmInfo.Enabled = true;
            calltofather.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(calltofather);

            var makeNewInstallation = server.CreateItem();
            makeNewInstallation.Start = DateTime.Today.AddDays(1).AddHours(0).AddMinutes(0).AddSeconds(0);
            makeNewInstallation.End = DateTime.Today.AddDays(4).AddHours(0).AddMinutes(0).AddSeconds(0);
            makeNewInstallation.Text = "Make New Installation";
            makeNewInstallation.Description = "The new Scheduler installation.";
            makeNewInstallation.Location = "work";
            makeNewInstallation.Category = KnownCalendarItemMarkers.CategoryImportant;
            makeNewInstallation.Status = KnownCalendarItemMarkers.StatusBusy;
            makeNewInstallation.AlarmInfo.Enabled = true;
            makeNewInstallation.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(makeNewInstallation);

            var doctorsAppointment = server.CreateItem();
            doctorsAppointment.Start = DateTime.Today.AddDays(1).AddHours(17).AddMinutes(0).AddSeconds(0);
            doctorsAppointment.End = DateTime.Today.AddDays(1).AddHours(17).AddMinutes(30).AddSeconds(0);
            doctorsAppointment.Text = "Doctor's Appointment";
            doctorsAppointment.Category = KnownCalendarItemMarkers.CategoryPersonal;
            doctorsAppointment.Status = KnownCalendarItemMarkers.StatusTentative;
            doctorsAppointment.AlarmInfo.Enabled = true;
            doctorsAppointment.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(doctorsAppointment);

            var meetmyfriends = server.CreateItem();
            meetmyfriends.Start = DateTime.Today.AddDays(1).AddHours(18).AddMinutes(0).AddSeconds(0);
            meetmyfriends.End = DateTime.Today.AddDays(1).AddHours(21).AddMinutes(0).AddSeconds(0);
            meetmyfriends.Text = "Meet my friends";
            meetmyfriends.Category = KnownCalendarItemMarkers.CategoryPersonal;
            meetmyfriends.Status = KnownCalendarItemMarkers.StatusBusy;
            meetmyfriends.AlarmInfo.Enabled = true;
            meetmyfriends.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(meetmyfriends);

            var mydaughtertheatricalperformance = server.CreateItem();
            mydaughtertheatricalperformance.Start = DateTime.Today.AddDays(2).AddHours(19).AddMinutes(0).AddSeconds(0);
            mydaughtertheatricalperformance.End = DateTime.Today.AddDays(2).AddHours(20).AddMinutes(30).AddSeconds(0);
            mydaughtertheatricalperformance.Text = "My daughter theatrical performance";
            mydaughtertheatricalperformance.Category = KnownCalendarItemMarkers.CategoryOther;
            mydaughtertheatricalperformance.Status = KnownCalendarItemMarkers.StatusAbsent;
            mydaughtertheatricalperformance.AlarmInfo.Enabled = true;
            mydaughtertheatricalperformance.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(mydaughtertheatricalperformance);

            var mybossparty = server.CreateItem();
            mybossparty.Start = DateTime.Today.AddDays(4).AddHours(18).AddMinutes(0).AddSeconds(0);
            mybossparty.End = DateTime.Today.AddDays(4).AddHours(22).AddMinutes(0).AddSeconds(0);
            mybossparty.Text = "My boss party";
            mybossparty.Category = KnownCalendarItemMarkers.CategoryPersonal;
            mybossparty.Status = KnownCalendarItemMarkers.StatusBusy;
            mybossparty.AlarmInfo.Enabled = true;
            mybossparty.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(mybossparty);

            var vacationpicnicwithbarbecue = server.CreateItem();
            vacationpicnicwithbarbecue.Start = DateTime.Today.AddDays(5).AddHours(8).AddMinutes(0).AddSeconds(0);
            vacationpicnicwithbarbecue.End = DateTime.Today.AddDays(5).AddHours(16).AddMinutes(0).AddSeconds(0);
            vacationpicnicwithbarbecue.Text = "Vacation picnic with barbecue";
            vacationpicnicwithbarbecue.Category = KnownCalendarItemMarkers.CategoryVacation;
            vacationpicnicwithbarbecue.Status = KnownCalendarItemMarkers.StatusAbsent;
            vacationpicnicwithbarbecue.AlarmInfo.Enabled = true;
            vacationpicnicwithbarbecue.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            vacationpicnicwithbarbecue.Recurrence.RecurrenceType = SchedulerServerRecurrenceType.Recurrence;
            vacationpicnicwithbarbecue.Recurrence.Range = SchedulerServerRecurrenceRange.Week;
            vacationpicnicwithbarbecue.Recurrence.MaxDate = DateTime.Today.AddDays(100);
            vacationpicnicwithbarbecue.Recurrence.DaysOfWeek = SchedulerWeekDay.Sunday;
            server.Add(vacationpicnicwithbarbecue);
        }

        /// <summary>
        /// Loads sample label events into the specified <see cref="CalendarItemSource"/>.
        /// </summary>
        /// <param name="server">The calendar item source to load the sample events into.</param>
        public static void LoadSampleLabelEvents(CalendarItemSource server)
        {
            DateTime start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 0, 0);
            var statuses = KnownCalendarItemMarkers.Statuses.ToArray();
            Random random = new Random();
            foreach (var category in KnownCalendarItemMarkers.Categories)
            {
                var item = server.CreateItem();

                item.Start = start;
                item.End = start.AddMinutes(10);
                item.Text = category.Title;
                item.Category = category;
                item.Status = statuses[random.Next(statuses.Length)];
                item.AlarmInfo.Enabled = true;
                item.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
                server.Add(item);

                start = start.AddMinutes(15);
            }
        }
    }
}
