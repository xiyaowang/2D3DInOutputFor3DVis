using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StudyMismatch2D3D.S23_Input_Android_Unity5 {
    public enum TangoState:byte {
        NotPresent,
        OutOfDate,
        Present
    }

    public enum XPCondition:byte {
        C0 = 0, //C0_MouseKeyboard_Screen
        C1 = 1,  //C1_Mouse3D_Screen
        C2 = 2,   //C2_Tablet_Screen
        C3 = 3,  //C3_MouseKeyboard_HoloLens
        C4 = 4,    //C4_Mouse3D_HoloLens
        C5 = 5  //C5_Tablet_HoloLens
    }

    public enum XPTask:byte {
        Docking = 0,
        Clipping = 1
    }
    public enum OutputCondition:byte {
        Screen = 0,
        HoloLens = 1
    }

    public enum InputCondition:byte {
        MouseKeybaord = 0,
        SpaceMouse = 1,
        Tablet = 2
    }

    public static class ExperimentConditions {
        public static int[][] TaskOrders = new int[][]{
            new int[]{0,1,2,3,4,5},
            new int[]{1,2,3,4,5,0},
            new int[]{2,3,4,5,0,1},
            new int[]{3,4,5,0,1,2},
            new int[]{4,5,0,1,2,3},
            new int[]{5,0,1,2,3,4},

            new int[]{0,1,2,3,4,5},
            new int[]{1,2,3,4,5,0},
            new int[]{2,3,4,5,0,1},
            new int[]{3,4,5,0,1,2},
            new int[]{4,5,0,1,2,3},
            new int[]{5,0,1,2,3,4},
        };

    }

}