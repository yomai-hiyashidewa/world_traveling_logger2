using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTravelLogger.Models.Enumeration
{
    public enum Transportationtype
    {
        Train,      // 
        Bus,        //
        AirPlane,   // 
        Ferry,      //
        Subway,     //
        Taxi,       //
        UBER,       //
        UBERMoto,   //
        Car,        //
        Tram,       //
        Bike,       //
        MotorCycle, //
        Tuktuk,     //
        BycleTaxi,  //
        Boat,       //
        Ropeway,   //
        Cesna,     //
        Track,     //
        Geepny,    //
        Walking,   // 徒歩
        LocalBus,   // busの中で同じ地区移動もしくは移動距離が10km以下
        MiddleDistanceBus,  // busの中で移動距離が100km未満
        LongDistanceBus,// その他
        LocalTrain, //  trainの中で同じ地区移動もしくは移動距離が10km以下
        MiddleDistanceTrain, // // busの中で移動距離が100km未満
        LongDistanceTrain,


    }
}
