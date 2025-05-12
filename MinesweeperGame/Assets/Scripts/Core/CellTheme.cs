using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Data/CellTheme", menuName = "Minesweeper/CellTheme", order = 0)]
    public class CellTheme : ScriptableObject
    {
        public Sprite unknown;
        public Sprite flagLight;
        public Sprite flagDark;

        public Sprite mineExplodedLight;
        public Sprite mineExplodedDark;

        //public Sprite questionMark;
        //public Sprite flagWrong;
        //public Sprite mineExploded;
        public Sprite disarmedLight;  // Add this
        public Sprite disarmedDark;   // Add this
        public Sprite backgroundDarkGreen;

        public Sprite mine1Light;
        public Sprite mine2Light;
        public Sprite mine3Light;
        public Sprite mine4Light;

        public Sprite mine1Dark;
        public Sprite mine2Dark;
        public Sprite mine3Dark;
        public Sprite mine4Dark;
    }
}