namespace SmartParkingApp.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CheckCreateSession()
        {
            ParkingManager manager = new ParkingManager(200, new MemoryLoad());
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 13, 0, 0);
            ParkingSession session = manager.EnterParking("232323");

            Assert.AreEqual(1, session.TicketNumber);
            Assert.AreEqual("232323", session.CarPlateNumber);
            Assert.AreEqual(ApplicationHelper.Instance.CurrentDate, session.EntryDt);         


        }

        [Test]
        public void CheckFreeParkingCapability()
        {
            ParkingManager manager = new ParkingManager(200, new MemoryLoad());
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 13, 0, 0);
            ParkingSession session = manager.EnterParking("232323");
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 13, 05, 0);
            decimal actualcost = manager.GetRemainingCost(session.TicketNumber);

            Assert.AreEqual(0, actualcost);
            Assert.IsTrue(manager.TryLeaveParkingWithTicket(session.TicketNumber, out ParkingSession p));
            CollectionAssert.Contains(manager.ArchivedSessions, session);
            CollectionAssert.DoesNotContain(manager.ActiveSessions,session);

        }
        [Test]
        public void CheckPaymentAfterParking()
        {
            ParkingManager manager = new ParkingManager(200,new MemoryLoad());
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 13, 0, 0);
            ParkingSession session = manager.EnterParking("232323");
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 14, 0, 0);
            decimal actualcost = manager.GetRemainingCost(session.TicketNumber);

            Assert.AreEqual(50, actualcost);
            Assert.IsFalse(manager.TryLeaveParkingWithTicket(session.TicketNumber,out ParkingSession p));
            CollectionAssert.Contains(manager.ActiveSessions, session);
            CollectionAssert.DoesNotContain(manager.ArchivedSessions, session);
        }
        [Test]
        public void CheckFreePeriodAfterPayment()
        {
            ParkingManager manager = new ParkingManager(200, new MemoryLoad());
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 13, 0, 0);
            ParkingSession session = manager.EnterParking("232323");
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 14, 0, 0);
            decimal actualcost = manager.GetRemainingCost(session.TicketNumber);
            manager.PayForParking(session.TicketNumber, actualcost);
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 14, 5, 0);
            decimal remaincost = manager.GetRemainingCost(session.TicketNumber);

            Assert.AreEqual(0, remaincost);
            Assert.IsTrue(manager.TryLeaveParkingWithTicket(session.TicketNumber, out ParkingSession p));
        }
        [Test]
        public void CheckAfterPaymentWithNotLeavinfPlace()
        {
            ParkingManager manager = new ParkingManager(200, new MemoryLoad());
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 13, 0, 0);
            ParkingSession session = manager.EnterParking("232323");
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 14, 0, 0);
            decimal actualcost = manager.GetRemainingCost(session.TicketNumber);
            manager.PayForParking(session.TicketNumber, actualcost);
            ApplicationHelper.Instance.CurrentDate = new DateTime(2023, 08, 23, 14, 25,0);
            decimal remaincost = manager.GetRemainingCost(session.TicketNumber);

            Assert.AreEqual(50, remaincost);
            Assert.IsFalse(manager.TryLeaveParkingWithTicket(session.TicketNumber, out ParkingSession p));
        }
    }
}