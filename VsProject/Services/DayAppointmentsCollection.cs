using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using VsProject.ViewModels;

namespace VsProject.Services
{

    public class DayAppointmentsCollection : Dictionary<DateOnly, ObservableCollection<AppointmentViewModel>>
    {
        private ObservableCollection<AppointmentViewModel>? _appointmentList;
        private readonly DateOnly _startDay;
        private readonly DateOnly _endDay;

        public ObservableCollection<AppointmentViewModel>? AppointmentList => _appointmentList;

        public DayAppointmentsCollection(ObservableCollection<AppointmentViewModel>? appointmentList, DateOnly startDay, DateOnly endDay)
        {
            _appointmentList = appointmentList;
            _startDay = startDay;
            _endDay = endDay;
            Initialize(startDay, endDay);
        }

        private void Initialize(DateOnly startDay, DateOnly endDay)
        {
            Clear();


            DateOnly currentDay = startDay;
            while (currentDay <= endDay)
            {
                Add(currentDay, new ObservableCollection<AppointmentViewModel>());
                currentDay = currentDay.AddDays(1);
            }

            foreach (var appointment in _appointmentList)
            {
                appointment.DateChanged += OnAppointment_DateChanged;
                DateOnly appointmentDate = DateOnly.FromDateTime(appointment.StartDateTime);
                if (appointmentDate >= startDay && appointmentDate <= endDay)
                {
                    this[appointmentDate].Add(appointment);
                }
            }
            _appointmentList.CollectionChanged += AppointmentsCollectionChanged;

        }

        private void OnAppointment_DateChanged(AppointmentViewModel appointment, DateOnly oldDate)
        {
            if (TryGetValue(oldDate, out var list))
            {
                this[oldDate].Remove(appointment);
                foreach(AppointmentViewModel item in list)
                {
                    item.OnPropertyChanged(nameof(item.StartTime));
                }
            }
            if (TryGetValue(appointment.Date, out _))
            {
                this[appointment.Date].Add(appointment);
            }
        }

        private void AppointmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    HandleAppointmentsAdded(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    HandleAppointmentsRemoved(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    HandleAppointmentsReplaced(e.NewItems, e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Initialize(_startDay, _endDay); 
                    break;
            }
        }

        private void HandleAppointmentsAdded(IList? newAppointments)
        {
            foreach (AppointmentViewModel appointment in newAppointments)
            {
                DateOnly appointmentDate = DateOnly.FromDateTime(appointment.StartDateTime);
                if (TryGetValue(appointmentDate, out _))
                {
                    this[appointmentDate].Add(appointment);
                }
            }
        }

        private void HandleAppointmentsRemoved(IList removedAppointments)
        {
            foreach (AppointmentViewModel appointment in removedAppointments)
            {
                DateOnly appointmentDate = DateOnly.FromDateTime(appointment.StartDateTime);
                if (TryGetValue(appointmentDate, out _))
                {
                    this[appointmentDate].Remove(appointment);
                }
            }
        }

        private void HandleAppointmentsReplaced(IList newAppointments, IList oldAppointments)
        {
            HandleAppointmentsRemoved(oldAppointments);
            HandleAppointmentsAdded(newAppointments);
        }

        //protected override void OnAppoiCollectionChanged(NotifyCollectionChangedEventArgs e)
        //{
        //        // Synchronize changes to the _appointmentList collection
        //    switch (e.Action)
        //    {
        //        case NotifyCollectionChangedAction.Add:
        //            foreach (Appointment appointment in e.NewItems)
        //            {
        //                _appointmentList.Add(appointment);
        //            }
        //            break;
        //        case NotifyCollectionChangedAction.Remove:
        //            foreach (Appointment appointment in e.OldItems)
        //            {
        //                _appointmentList.Remove(appointment);
        //            }
        //            break;
        //        case NotifyCollectionChangedAction.Replace:
        //            foreach (Appointment appointment in e.OldItems)
        //            {
        //                _appointmentList.Remove(appointment);
        //            }
        //            foreach (Appointment appointment in e.NewItems)
        //            {
        //                _appointmentList.Add(appointment);
        //            }
        //            break;
        //        case NotifyCollectionChangedAction.Reset:
        //            _appointmentList.Clear();
        //            foreach (var dayAppointments in this)
        //            {
        //                foreach (Appointment appointment in dayAppointments)
        //                {
        //                    _appointmentList.Add(appointment);
        //                }
        //            }
        //            break;
        //    }

        //    base.OnCollectionChanged(e);
        //}



        //protected override void ClearItems()
        //{
        //    _appointmentList.Clear();
        //    base.ClearItems();
        //}

        //protected override void RemoveItem(int index)
        //{
        //    var appointments = this[index];

        //    foreach (var appointment in appointments.ToList())
        //    {
        //        _appointmentList.Remove(appointment);
        //    }

        //    base.RemoveItem(index);

        //}

        //protected override void InsertItem(int index, ObservableCollection<Appointment> item)
        //{
        //    //foreach (var appointment in item)
        //    //{
        //    //    _appointmentList.Add(appointment);
        //    //}
        //    base.InsertItem(index, item);
        //}

        //protected override void SetItem(int index, ObservableCollection<Appointment> item)
        //{
        //    var removedAppointments = this[index];

        //    // Remove the appointments that were replaced
        //    foreach (var appointment in removedAppointments)
        //    {
        //        _appointmentList.Remove(appointment);
        //    }

        //    // Add the new appointments
        //    foreach (var appointment in item)
        //    {
        //        _appointmentList.Add(appointment);
        //    }
        //    base.SetItem(index, item);

        //}

        ////protected virtual void MoveItem(int oldIndex, int newIndex)


    }
}
