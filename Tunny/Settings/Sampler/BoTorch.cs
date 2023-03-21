namespace Tunny.Settings.Sampler
{
    /// <summary>
    /// https://optuna.readthedocs.io/en/stable/reference/generated/optuna.integration.BoTorchSampler.html
    /// </summary>
    public class BoTorch
    {
        public int NStartupTrials { get; set; } = 10;
        public TorchDevice Device { get; set; } = TorchDevice.cpu;
    }

    public enum TorchDevice
    {
        cpu,
        cuda
    }
}
