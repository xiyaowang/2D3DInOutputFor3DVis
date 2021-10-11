using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudyMismatch2D3D.S23_Input_PC {

    public enum XPTask:byte {
        Docking = 0,
        Clipping = 1
    }

    public enum XPCondition:byte {
        C0 = 0, //C0_MouseKeyboard_Screen
        C1 = 1,  //C1_Mouse3D_Screen
        C2 = 2,   //C2_Tablet_Screen
        C3 = 3,  //C3_MouseKeyboard_HoloLens
        C4 = 4,    //C4_Mouse3D_HoloLens
        C5 = 5  //C5_Tablet_HoloLens
    }

    public enum InputCondition:byte {
        MouseKeybaord = 0,
        SpaceMouse = 1,
        Tablet = 2
    }

    public enum OutputCondition:byte {
        Screen = 0,
        HoloLens = 1
    }

    public static class ExperimentConditions {

        public static XPCondition[][] XPConditions = new XPCondition[][]{
            new XPCondition[]{XPCondition.C0,XPCondition.C3,XPCondition.C1,XPCondition.C4,XPCondition.C2,XPCondition.C5},
            new XPCondition[]{XPCondition.C0,XPCondition.C3,XPCondition.C2,XPCondition.C5,XPCondition.C1,XPCondition.C4},
            new XPCondition[]{XPCondition.C1,XPCondition.C4,XPCondition.C0,XPCondition.C3,XPCondition.C2,XPCondition.C5},
            new XPCondition[]{XPCondition.C1,XPCondition.C4,XPCondition.C2,XPCondition.C5,XPCondition.C0,XPCondition.C3},
            new XPCondition[]{XPCondition.C2,XPCondition.C5,XPCondition.C0,XPCondition.C3,XPCondition.C1,XPCondition.C4},
            new XPCondition[]{XPCondition.C2,XPCondition.C5,XPCondition.C1,XPCondition.C4,XPCondition.C0,XPCondition.C3},

            new XPCondition[]{XPCondition.C3,XPCondition.C0,XPCondition.C4,XPCondition.C1,XPCondition.C5,XPCondition.C2},
            new XPCondition[]{XPCondition.C3,XPCondition.C0,XPCondition.C5,XPCondition.C2,XPCondition.C4,XPCondition.C1},
            new XPCondition[]{XPCondition.C4,XPCondition.C1,XPCondition.C3,XPCondition.C0,XPCondition.C5,XPCondition.C2},
            new XPCondition[]{XPCondition.C4,XPCondition.C1,XPCondition.C5,XPCondition.C2,XPCondition.C3,XPCondition.C0},
            new XPCondition[]{XPCondition.C2,XPCondition.C5,XPCondition.C3,XPCondition.C0,XPCondition.C4,XPCondition.C1},
            new XPCondition[]{XPCondition.C2,XPCondition.C5,XPCondition.C4,XPCondition.C1,XPCondition.C3,XPCondition.C0},
        };


        public static InputCondition[][] InputOrders = new InputCondition[][]{
           new InputCondition[]{InputCondition.MouseKeybaord, InputCondition.MouseKeybaord, InputCondition.SpaceMouse,InputCondition.SpaceMouse, InputCondition.Tablet, InputCondition.Tablet },
           new InputCondition[]{InputCondition.MouseKeybaord, InputCondition.MouseKeybaord, InputCondition.Tablet,InputCondition.Tablet, InputCondition.SpaceMouse, InputCondition.SpaceMouse },
           new InputCondition[]{InputCondition.SpaceMouse, InputCondition.SpaceMouse, InputCondition.MouseKeybaord,InputCondition.MouseKeybaord, InputCondition.Tablet, InputCondition.Tablet },
           new InputCondition[]{InputCondition.SpaceMouse, InputCondition.SpaceMouse, InputCondition.Tablet,InputCondition.Tablet, InputCondition.MouseKeybaord, InputCondition.MouseKeybaord },
           new InputCondition[]{InputCondition.Tablet, InputCondition.Tablet, InputCondition.MouseKeybaord,InputCondition.MouseKeybaord, InputCondition.SpaceMouse, InputCondition.SpaceMouse },
           new InputCondition[]{InputCondition.Tablet, InputCondition.Tablet, InputCondition.SpaceMouse,InputCondition.SpaceMouse, InputCondition.MouseKeybaord, InputCondition.MouseKeybaord },

           new InputCondition[]{InputCondition.MouseKeybaord, InputCondition.MouseKeybaord, InputCondition.SpaceMouse,InputCondition.SpaceMouse, InputCondition.Tablet, InputCondition.Tablet },
           new InputCondition[]{InputCondition.MouseKeybaord, InputCondition.MouseKeybaord, InputCondition.Tablet,InputCondition.Tablet, InputCondition.SpaceMouse, InputCondition.SpaceMouse },
           new InputCondition[]{InputCondition.SpaceMouse, InputCondition.SpaceMouse, InputCondition.MouseKeybaord,InputCondition.MouseKeybaord, InputCondition.Tablet, InputCondition.Tablet },
           new InputCondition[]{InputCondition.SpaceMouse, InputCondition.SpaceMouse, InputCondition.Tablet,InputCondition.Tablet, InputCondition.MouseKeybaord, InputCondition.MouseKeybaord },
           new InputCondition[]{InputCondition.Tablet, InputCondition.Tablet, InputCondition.MouseKeybaord,InputCondition.MouseKeybaord, InputCondition.SpaceMouse, InputCondition.SpaceMouse },
           new InputCondition[]{InputCondition.Tablet, InputCondition.Tablet, InputCondition.SpaceMouse,InputCondition.SpaceMouse, InputCondition.MouseKeybaord, InputCondition.MouseKeybaord },
        };

        public static OutputCondition[][] OutputOrders = new OutputCondition[][]{
           new OutputCondition[]{OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens},
           new OutputCondition[]{OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens},
           new OutputCondition[]{OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens},
           new OutputCondition[]{OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens},
           new OutputCondition[]{OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens},
           new OutputCondition[]{OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens},

           new OutputCondition[]{OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen},
           new OutputCondition[]{OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen},
           new OutputCondition[]{OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen},
           new OutputCondition[]{OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen},
           new OutputCondition[]{OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen},
           new OutputCondition[]{OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen,OutputCondition.HoloLens,OutputCondition.Screen},
        };

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