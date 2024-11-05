namespace UDEV.SPM
{
    public enum PoolerTarget
    {
        NONE,
        PLAYER,
        AI,
        AISPAWN,
        VFX,
        COLLECTABLE,
        OTHER
    }

    public static class Constants
    {
        #region EDITOR_SCRIPTS_CONSTANTS
        public const string PROJECT_FOLDER_NAME = "UDEV/Simple Pooling Manager";
        public const string EDITOR_DATA_PATH = "Assets/" + PROJECT_FOLDER_NAME + "/Scripts/Editor/Data/";
        #endregion
    }
}