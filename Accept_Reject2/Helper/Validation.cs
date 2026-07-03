using System;

namespace Accept_Reject2.Helper
{
    public static class ValidationHelper
    {
        //validate width of the image
        public static bool ValidateWidth(int width)
        {
            return width >= 20 && width <= 350;
        }
        //validate height of the image
        public static bool ValidateHeight(int height)
        {
            return height >= 20 && height <= 350;
        }
        //validate Angle of the image
        public static bool ValidateAngle(float angle)
        {
            return angle >= -360 && angle <= 360;
        }
        //validate centerX of the image
        public static bool ValidateCenter(float x, float y, float maxX, float maxY)
        {
            return x >= -maxX && x <= maxX && y >= -maxY && y <= maxY;
        }


    }
}