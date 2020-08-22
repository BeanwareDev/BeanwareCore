namespace BeanwareCore.Src.Data.Utility
{
    /// <summary>Utility data class for calculating and storing averages</summary>
    public class BAverage
    {
        // Variables
        public int Count { get; private set; }
        public float Sum { get; private set; }
        public float Value { get; private set; }

        // Constructors
        public BAverage()
        {
            Reset();
        }

        // Methods
        /// <summary> Resets the average to 0 </summary>
        public void Reset()
        {
            Count = 0;
            Sum = 0;
            Value = 0;
        }
        /// <summary> Adds the value to the average </summary>
        public void Add(float value)
        {
            Count++;
            Sum += value;
            Value = Sum / Count;
        }
    }
}
