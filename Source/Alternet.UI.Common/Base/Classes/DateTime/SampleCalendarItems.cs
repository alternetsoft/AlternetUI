using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a set of sample calendar items for demonstration purposes.
    /// </summary>
    public static class SampleCalendarItems
    {
        /// <summary>
        /// Loads sample calendar items into the specified <see cref="SchedulerDocument"/>.
        /// </summary>
        /// <param name="server"></param>
        public static void LoadSampleData(SchedulerDocument server)
        {
            var createNewDemo = server.CreateItem();
            createNewDemo.Rule.ValidFromDate = DateUtils.Today();
            createNewDemo.Rule.ValidToDate = DateUtils.Today().AddDays(2);
            createNewDemo.Text = "Create New Demo";
            createNewDemo.Location = "work";
            createNewDemo.Category = KnownCalendarItemMarkers.CategoryImportant;
            createNewDemo.Status = KnownCalendarItemMarkers.StatusBusy;
            createNewDemo.AlarmInfo.Enabled = true;
            createNewDemo.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            createNewDemo.Description = "The new Scheduler demo.";

            TestIsValid(createNewDemo);

            server.Add(createNewDemo);

            var gototheBeach = server.CreateItem();
            gototheBeach.Rule.ValidFromDate = DateUtils.Today();
            gototheBeach.Rule.StartTime = new (18, 0);
            gototheBeach.Rule.ValidToDate = DateUtils.Today();
            gototheBeach.Rule.Duration = TimeSpan.FromHours(3);
            gototheBeach.Text = "Go to the Beach";
            gototheBeach.Description = "Try to surf-riding.";
            gototheBeach.Location = "home";
            gototheBeach.Category = KnownCalendarItemMarkers.CategoryPersonal;
            gototheBeach.Status = KnownCalendarItemMarkers.StatusFree;
            gototheBeach.AlarmInfo.Enabled = true;
            gototheBeach.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(gototheBeach);

            var calltofather = server.CreateItem();
            calltofather.Rule.ValidFromDate = DateUtils.Today();
            calltofather.Rule.StartTime = new (20, 0);
            calltofather.Rule.ValidToDate = DateUtils.Today();
            calltofather.Rule.Duration = TimeSpan.FromMinutes(30);
            calltofather.Text = "Call to father";
            calltofather.Category = KnownCalendarItemMarkers.CategoryPhoneCall;
            calltofather.Status = KnownCalendarItemMarkers.StatusUnknown;
            calltofather.AlarmInfo.Enabled = true;
            calltofather.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(calltofather);

            var makeNewInstallation = server.CreateItem();
            makeNewInstallation.Rule.ValidFromDate = DateUtils.Tomorrow();

            makeNewInstallation.Rule.ValidToDate = DateUtils.Today().AddDays(4);
            makeNewInstallation.Text = "Make New Installation";
            makeNewInstallation.Description = "The new Scheduler installation.";
            makeNewInstallation.Location = "work";
            makeNewInstallation.Category = KnownCalendarItemMarkers.CategoryImportant;
            makeNewInstallation.Status = KnownCalendarItemMarkers.StatusBusy;
            makeNewInstallation.AlarmInfo.Enabled = true;
            makeNewInstallation.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(makeNewInstallation);

            var doctorsAppointment = server.CreateItem();
            doctorsAppointment.Rule.ValidFromDate = DateUtils.Tomorrow();
            doctorsAppointment.Rule.ValidToDate = DateUtils.Tomorrow();
            doctorsAppointment.Rule.StartTime = new(17, 0);
            doctorsAppointment.Rule.Duration = TimeSpan.FromMinutes(30);
            doctorsAppointment.Text = "Doctor's Appointment";
            doctorsAppointment.Category = KnownCalendarItemMarkers.CategoryPersonal;
            doctorsAppointment.Status = KnownCalendarItemMarkers.StatusTentative;
            doctorsAppointment.AlarmInfo.Enabled = true;
            doctorsAppointment.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(doctorsAppointment);

            var meetmyfriends = server.CreateItem();
            meetmyfriends.Rule.ValidFromDate = DateUtils.Tomorrow();
            meetmyfriends.Rule.ValidToDate = DateUtils.Tomorrow();
            meetmyfriends.Rule.StartTime = new(18, 0);
            meetmyfriends.Rule.Duration = TimeSpan.FromHours(3);
            meetmyfriends.Text = "Meet my friends";
            meetmyfriends.Category = KnownCalendarItemMarkers.CategoryPersonal;
            meetmyfriends.Status = KnownCalendarItemMarkers.StatusBusy;
            meetmyfriends.AlarmInfo.Enabled = true;
            meetmyfriends.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(meetmyfriends);

            var mydaughtertheatricalperformance = server.CreateItem();
            mydaughtertheatricalperformance.Rule.ValidFromDate = DateUtils.Today().AddDays(2);
            mydaughtertheatricalperformance.Rule.ValidToDate = DateUtils.Today().AddDays(2);
            mydaughtertheatricalperformance.Rule.StartTime = new(19, 0);
            mydaughtertheatricalperformance.Rule.Duration = TimeSpan.FromHours(1.5);
            mydaughtertheatricalperformance.Text = "My daughter theatrical performance";
            mydaughtertheatricalperformance.Category = KnownCalendarItemMarkers.CategoryOther;
            mydaughtertheatricalperformance.Status = KnownCalendarItemMarkers.StatusAbsent;
            mydaughtertheatricalperformance.AlarmInfo.Enabled = true;
            mydaughtertheatricalperformance.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(mydaughtertheatricalperformance);

            var mybossparty = server.CreateItem();
            mybossparty.Rule.ValidFromDate = DateUtils.Today().AddDays(4);
            mybossparty.Rule.ValidToDate = DateUtils.Today().AddDays(4);
            mybossparty.Rule.StartTime = new(18, 0);
            mybossparty.Rule.Duration = TimeSpan.FromHours(4);
            mybossparty.Text = "My boss party";
            mybossparty.Category = KnownCalendarItemMarkers.CategoryPersonal;
            mybossparty.Status = KnownCalendarItemMarkers.StatusBusy;
            mybossparty.AlarmInfo.Enabled = true;
            mybossparty.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
            server.Add(mybossparty);

            var vacationpicnicwithbarbecue = server.CreateItem();
            vacationpicnicwithbarbecue.Rule.ValidFromDate = DateUtils.Today().AddDays(5);
            vacationpicnicwithbarbecue.Rule.ValidToDate = DateUtils.Today().AddDays(100);
            vacationpicnicwithbarbecue.Rule.StartTime = new(8, 0);
            vacationpicnicwithbarbecue.Rule.Duration = TimeSpan.FromHours(8);
            vacationpicnicwithbarbecue.Text = "Vacation picnic with barbecue";
            vacationpicnicwithbarbecue.Category = KnownCalendarItemMarkers.CategoryVacation;
            vacationpicnicwithbarbecue.Status = KnownCalendarItemMarkers.StatusAbsent;
            vacationpicnicwithbarbecue.AlarmInfo.Enabled = true;
            vacationpicnicwithbarbecue.AlarmInfo.Advance = TimeSpan.FromMinutes(5);

            vacationpicnicwithbarbecue.Rule.Kind = ScheduleRepeatPattern.Weekly;
            vacationpicnicwithbarbecue.Rule.WeeklyRule.WeekDays = DaysOfWeek.Sunday;
            server.Add(vacationpicnicwithbarbecue);
        }

        /// <summary>
        /// Loads sample label events into the specified <see cref="SchedulerDocument"/>.
        /// </summary>
        /// <param name="server">The calendar item source to load the sample events into.</param>
        public static void LoadSampleLabelEvents(SchedulerDocument server)
        {
            TimeOnly start = new (10, 0);
            var statuses = KnownCalendarItemMarkers.Statuses.ToArray();
            Random random = new ();
            foreach (var category in KnownCalendarItemMarkers.Categories)
            {
                var item = server.CreateItem();

                item.Rule.ValidFromDate = DateUtils.Today();
                item.Rule.ValidToDate = DateUtils.Today();
                item.Rule.StartTime = start;
                item.Rule.Duration = TimeSpan.FromMinutes(10);
                item.Text = category.Title;
                item.Category = category;
                item.Status = statuses[random.Next(statuses.Length)];
                item.AlarmInfo.Enabled = true;
                item.AlarmInfo.Advance = TimeSpan.FromMinutes(5);
                server.Add(item);

                start = start.AddMinutes(15);
            }
        }

        [Conditional("DEBUG")]
        private static void TestIsValid(CalendarItem item)
        {
#pragma warning disable
            var instance = item.GetThisYearInstances().FirstOrDefault();

            var isValid = instance.IsValid;

            instance.Item.Description = "new text";

            isValid = instance.IsValid;

            instance = item.GetThisYearInstances().FirstOrDefault();
            isValid = instance.IsValid;

            instance.Item.Rule.Kind = ScheduleRepeatPattern.Daily;
            isValid = instance.IsValid;
#pragma warning restore
        }
    }
}
