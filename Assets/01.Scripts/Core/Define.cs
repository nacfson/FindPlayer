using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace Core{
    public class Define{
        private static Camera _mainCam;
        public static int GameSceneIndex = 1;
        public static int RoomIndex = 0;
        public static Camera MainCam{
            get{
                if(_mainCam == null){
                    _mainCam = Camera.main;
                }
                return _mainCam;
            }
        }
        
    }
}
