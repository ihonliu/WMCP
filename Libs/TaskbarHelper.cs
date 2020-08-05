using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WMCP.Libs {
    public static class TaskbarHelper {
        public enum TaskBarLocation { TOP, BOTTOM, LEFT, RIGHT }
        public static TaskBarLocation GetTaskBarLocation() {
            if (Screen.PrimaryScreen.WorkingArea.Width == Screen.PrimaryScreen.Bounds.Width) 
                return (Screen.PrimaryScreen.WorkingArea.Top > 0) ? TaskBarLocation.TOP : TaskBarLocation.BOTTOM;
            else
                return (Screen.PrimaryScreen.WorkingArea.Left > 0) ? TaskBarLocation.LEFT : TaskBarLocation.RIGHT;
        }
        public static Point GetTrayPopupWindowPostion(int width, int height) {
            var tl = GetTaskBarLocation();
            int top = 0, left = 0;
            switch (tl) {
                case TaskBarLocation.TOP:
                    top = Screen.PrimaryScreen.WorkingArea.Y;
                    left = Screen.PrimaryScreen.WorkingArea.Width-width;
                    break;
                
                case TaskBarLocation.LEFT:
                    top = Screen.PrimaryScreen.WorkingArea.Height-height;
                    left = Screen.PrimaryScreen.WorkingArea.X;
                    break;
                case TaskBarLocation.BOTTOM:
                case TaskBarLocation.RIGHT:
                default:
                    top = Screen.PrimaryScreen.WorkingArea.Height-height;
                    left = Screen.PrimaryScreen.WorkingArea.Width-width;
                    break;
            }
            return new Point(left, top);
        }
    }
}
