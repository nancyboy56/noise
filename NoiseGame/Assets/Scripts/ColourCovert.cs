using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct rgb {
    public double r;       // a fraction between 0 and 1
    public double g;       // a fraction between 0 and 1
    public double b;       // a fraction between 0 and 1
}


 public struct hsv {
    public double h;       // angle in degrees
    public double s;       // a fraction between 0 and 1
    public double v;       // a fraction between 0 and 1
} 

public class ColourCovert 
{
    
    public double[] rgb2hsv(double r, double g, double b)
    {
        rgb input = new rgb();
        double[] outputArray = new double[3];
        input.r = r;
        input.b = b;
        input.g = g;

        hsv output;
        double min, max, delta;

        min = input.r < input.g ? input.r: input.g;
        min = min < input.b? min  : input.b;

        max = input.r > input.g ? input.r: input.g;
        max = max > input.b? max  : input.b;

        output.v = max;                                // v
        delta = max - min;
        if (delta < 0.00001)
        {
            output.s = 0;
            output.h = 0; // undefined, maybe nan?
            outputArray[0] = output.h;
            outputArray[1] = output.s;
            outputArray[2] = output.v;
            return outputArray;
        }
        if (max > 0.0)
        { // NOTE: if Max is == 0, this divide would cause a crash
            output.s = (delta / max);                  // s
        }
        else
        {
            // if max is 0, then r = g = b = 0              
            // s = 0, h is undefined
            output.s = 0.0;
            output.h = 0.0;
            // its now undefined
            outputArray[0] = output.h;
            outputArray[1] = output.s;
            outputArray[2] = output.v;
            return outputArray;
        }

        // > is bogus, just keeps compilor happy
        if (input.r >= max )
            // between yellow & magenta
            output.h = (input.g - input.b ) / delta;        
        else if (input.g >= max )
            // between cyan & yellow
            output.h = 2.0 + (input.b - input.r ) / delta;  
        else
            // between magenta & cyan
            output.h = 4.0 + (input.r - input.g ) / delta;  

        // degrees
        output.h *= 60.0;  

        if (output.h < 0.0)
            output.h += 360.0;

        outputArray[0] = output.h;
        outputArray[1] = output.s;
        outputArray[2] = output.v;
        return outputArray;
    }


    public double[] hsv2rgb(double h, double s, double v)
    {
        hsv input = new hsv();
        double[] outputArray = new double[3];
        input.h = h;
        input.s = s;
        input.v = v;

        double hh, p, q, t, ff;
        long i;
        rgb output;

        if (input.s <= 0.0) {       // < is bogus, just shuts up warnings
            output.r = input.v;
            output.g = input.v;
            output.b = input.v;
            outputArray[0] = output.r;
            outputArray[1] = output.g;
            outputArray[2] = output.b;
            return outputArray;
        }
        hh = input.h;

        if (hh >= 360.0) hh = 0.0;
        hh /= 60.0;
        i = (long)hh;
        ff = hh - i;
        p = input.v * (1.0 - input.s);
        q = input.v * (1.0 - (input.s* ff));
        t = input.v * (1.0 - (input.s * (1.0 - ff)));

        switch (i)
        {
            case 0:
                output.r = input.v;
                output.g = t;
                output.b = p;
                break;
            case 1:
                output.r = q;
                output.g = input.v;
                output.b = p;
                break;
            case 2:
                output.r = p;
                output.g = input.v;
                output.b = t;
                break;

            case 3:
                output.r = p;
                output.g = q;
                output.b = input.v;
                break;
            case 4:
                output.r = t;
                output.g = p;
                output.b = input.v;
                break;
            case 5:
            default:
                output.r = input.v;
                output.g = p;
                output.b = q;
                break;
        }
        outputArray[0] = output.r;
        outputArray[1] = output.g;
        outputArray[2] = output.b;
        return outputArray;
    }
}
