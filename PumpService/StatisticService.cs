using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PumpService
{
    public class StatisticService : IStatisticService
    {
        public int SuccessTacts { get; set; }// { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ErrorTacts { get; set; }//{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int AllTacts { get; set; }//{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}