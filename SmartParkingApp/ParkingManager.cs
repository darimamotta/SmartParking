using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartParkingApp
{
    public class ParkingManager
    {
        private int parkingCapacity;
        private List<ParkingSession> activeSessions = new List<ParkingSession>();
        private List<ParkingSession> archivedSessions = new List<ParkingSession> ();
        private List<Tariff> tariffs = new List<Tariff> ();
        public List<ParkingSession> ActiveSessions{ get { return activeSessions; } }
        public List<ParkingSession> ArchivedSessions { get { return archivedSessions; } }
        public int FreePlace { get { return parkingCapacity - activeSessions.Count; } }
        public int FreePeriod 
        { 
            get
            { 
                Tariff tariff = tariffs.FirstOrDefault (x=>x.Rate==0);
                if (tariff != null)
                {
                    return tariff.Minutes;
                }
                else
                    return 5;
            } 
        }
        private int count=1;

        public ParkingManager(int parkingCapacity, ITariffLoad tariffLoad)
        {
            this.parkingCapacity = parkingCapacity;
            tariffs = tariffLoad.LoadTariff();
        }

        /* BASIC PART */
        public ParkingSession EnterParking(string carPlateNumber)
        {
            if (FreePlace == 0)
            {
                return null;

            }
            if (activeSessions.Any(x=>x.CarPlateNumber==carPlateNumber))
                throw new Exception("The car with this number has already parked!");
            ParkingSession parkingSession = new ParkingSession(ApplicationHelper.Instance.CurrentDate, carPlateNumber, count);
            count++;
            if (count == 100000)
            { count = 1; }
            activeSessions.Add(parkingSession);
            return parkingSession;
            
          
            /* Check that there is a free parking place (by comparing the parking capacity 
             * with the number of active parking sessions). If there are no free places, return null
             
             * 
             * Also check that there are no existing active sessions with the same car plate number,
             * if such session exists, also return null
             * 
             * Otherwise:
             * Create a new Parking session, fill the following properties:
             * EntryDt = current date time
             * CarPlateNumber = carPlateNumber (from parameter)
             * TicketNumber = unused parking ticket number = generate this programmatically
             * 
             * Add the newly created session to the list of active sessions
             * 
             * Advanced task:
             * Link the new parking session to an existing user by car plate number (if such user exists)            
             */
           
        }

        public bool TryLeaveParkingWithTicket(int ticketNumber, out ParkingSession session)
        {
            ParkingSession newsession = activeSessions.FirstOrDefault(x=>x.TicketNumber==ticketNumber);
            if (newsession==null)
            { throw new Exception("Session is null!"); }
            int minutes = 0;
            if (newsession.PaymentDt.HasValue)
            {
                minutes = (int)ApplicationHelper.Instance.CurrentDate.Subtract(newsession.PaymentDt.Value).TotalMinutes;
            }
            else
                minutes = (int)ApplicationHelper.Instance.CurrentDate.Subtract(newsession.EntryDt).TotalMinutes;
            if(minutes < FreePeriod)
            {
                activeSessions.Remove(newsession);
                archivedSessions.Add(newsession);
                newsession.ExitDt = ApplicationHelper.Instance.CurrentDate;
                session = newsession;
                return true;
            }
            session = null;
            return false;



            /*
             * Check that the car leaves parking within the free leave period
             * from the PaymentDt (or if there was no payment made, from the EntryDt)
             * 1. If yes:
             *   1.1 Complete the parking session by setting the ExitDt property
             *   1.2 Move the session from the list of active sessions to the list of past sessions             * 
             *   1.3 return true and the completed parking session object in the out parameter
             * 
             * 2. Otherwise, return false, session = null
             */
            
        }        

        public decimal GetRemainingCost(int ticketNumber)
        {
            ParkingSession newsession = activeSessions.FirstOrDefault(x => x.TicketNumber == ticketNumber);
            if (newsession == null)
            { throw new Exception("Session is null!"); }
            int minutes = 0;
            if (newsession.PaymentDt.HasValue)
            {
                minutes = (int)ApplicationHelper.Instance.CurrentDate.Subtract(newsession.PaymentDt.Value).TotalMinutes;
            }
            else
                minutes = (int)ApplicationHelper.Instance.CurrentDate.Subtract(newsession.EntryDt).TotalMinutes;
            Tariff tariff = tariffs.OrderBy(x=>x.Minutes).FirstOrDefault(y=>y.Minutes>=minutes);
            if (tariff == null)
            {
                tariff = tariffs.Last();
            }
            return tariff.Rate;

            /* Return the amount to be paid for the parking
             * If a payment had already been made but additional charge was then given
             * because of a late exit, this method should return the amount 
             * that is yet to be paid (not the total charge)
             */
           
        }

        public void PayForParking(int ticketNumber, decimal amount)
        {
            ParkingSession session = activeSessions.FirstOrDefault(y => y.TicketNumber == ticketNumber);
            session.TotalPayment=amount;
            session.PaymentDt = ApplicationHelper.Instance.CurrentDate;

            /*
             * Save the payment details in the corresponding parking session
             * Set PaymentDt to current date and time
             * 
             * For simplicity we won't make any additional validation here and always
             * assume that the parking charge is paid in full
             */
        }

        /* ADDITIONAL TASK 2 */
        public bool TryLeaveParkingByCarPlateNumber(string carPlateNumber, out ParkingSession session)
        {
            /* There are 3 scenarios for this method:
            
            1. The user has not made any payments but leaves the parking within the free leave period
            from EntryDt:
               1.1 Complete the parking session by setting the ExitDt property
               1.2 Move the session from the list of active sessions to the list of past sessions             * 
               1.3 return true and the completed parking session object in the out parameter
            
            2. The user has already paid for the parking session (session.PaymentDt != null):
            Check that the current time is within the free leave period from session.PaymentDt
               2.1. If yes, complete the session in the same way as in the previous scenario
               2.2. If no, return false, session = null

            3. The user has not paid for the parking session:            
            3a) If the session has a connected user (see advanced task from the EnterParking method):
            ExitDt = PaymentDt = current date time; 
            TotalPayment according to the tariff table:            
            
            IMPORTANT: before calculating the parking charge, subtract FreeLeavePeriod 
            from the total number of minutes passed since entry
            i.e. if the registered visitor enters the parking at 10:05
            and attempts to leave at 10:25, no charge should be made, otherwise it would be unfair
            to loyal customers, because an ordinary printed ticket could be inserted in the payment
            kiosk at 10:15 (no charge) and another 15 free minutes would be given (up to 10:30)

            return the completed session in the out parameter and true in the main return value

            3b) If there is no connected user, set session = null, return false (the visitor
            has to insert the parking ticket and pay at the kiosk)
            */
            throw new NotImplementedException();
        }
    }
}
