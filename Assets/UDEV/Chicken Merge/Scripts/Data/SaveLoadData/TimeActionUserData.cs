namespace UDEV.ChickenMerge
{
    [System.Serializable]
    public class TimeActionUserData
    {
        public string key;
        public string value;

        public TimeActionUserData(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

}