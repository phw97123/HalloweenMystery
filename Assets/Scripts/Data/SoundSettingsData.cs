namespace Components.Data
{
    public struct SoundSettingsData
    {
        public float soundVolume;
        public float musicVolume;
        public bool isSoundOn;
        public bool isMusicOn;

        public SoundSettingsData(float musicVolume, float soundVolume, bool isMusicOn, bool isSoundOn)
        {
            this.isMusicOn = isMusicOn;
            this.isSoundOn = isSoundOn;
            this.musicVolume = musicVolume;
            this.soundVolume = soundVolume;
        }
    }
}