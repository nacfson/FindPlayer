using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
namespace Core{
    public class Define{
        private static Camera _mainCam;
        private static CinemachineVirtualCamera _cmCam;
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

        public static CinemachineVirtualCamera CMCam
        {
            get
            {
                if (_cmCam == null)
                {
                    _cmCam = GameObject.Find("CMCam").GetComponent<CinemachineVirtualCamera>();
                }
                return _cmCam;
            }
        }
        
    }
}
