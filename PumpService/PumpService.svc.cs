using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PumpService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PumpService : IPumpService
    {
        private readonly IScriptService _script;
        private readonly IStatisticService _statistic;
        private readonly ISettingsService _settings;
        private IPumpServiceCallback _callback
        {
            get
            {
                if (OperationContext.Current != null)
                    return OperationContext.Current.GetCallbackChannel<IPumpServiceCallback>();
                else 
                    return null;
            }
        }

        public PumpService()
        {
            _statistic = new StatisticService();
            _settings = new SettingsService();
            _script = new ScriptService(_statistic, _settings, _callback);
        }

        public void RunScript()
        {
            _script.Run(10);
        }

        public void UpdateAndCompile(string fileName)
        {
            _settings.FileName= fileName;
            _script.Compile();
        }
    }
}
