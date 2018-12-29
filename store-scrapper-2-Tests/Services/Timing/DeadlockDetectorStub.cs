using store_scrapper_2.Configuration;
using store_scrapper_2.Services;
using store_scrapper_2.Services.Timing;

namespace store_scrapper_2_Tests.Services.Timing
{
  public class DeadlockDetectorStub : DeadlockDetector
  {
    public bool ProgramAborted { get; private set; }
    
    public DeadlockDetectorStub(IConfigurationReader configurationReader, ITimerFactory timerFactory) 
      : base(configurationReader, timerFactory) { }

    protected override void AbortProgram() => ProgramAborted = true;
  }
}