namespace Stately.Tests.MockData
{
    public class EntryExitTestObject
    {
        private int _amountOfEntries;

        public bool ExitCalledBeforeSecondEntry { get; private set; }

        public void OnEntry()
        {
            _amountOfEntries++;
        }

        public void OnExit()
        {
            if (_amountOfEntries < 2)
                ExitCalledBeforeSecondEntry = true;
        }
    }
}