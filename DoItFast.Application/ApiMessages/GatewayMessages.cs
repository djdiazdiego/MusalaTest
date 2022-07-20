using System.ComponentModel;

namespace DoItFast.Application.ApiMessages
{
    public enum GatewayMessages
    {
        /// <summary>
        /// Incorrect composition for Serial Number
        /// </summary>
        [Description("Only numbers and uppercase characters are allowed")]
        SerialNumberIncorrectComposition = 1,
        /// <summary>
        /// Exceeded Peripheral Device number
        /// </summary>
        [Description("No more than 10 peripheral devices are allowed for a gateway")]
        ExceededPeripheralDeviceNumber = 2,
        /// <summary>
        /// Nonexistent Peripheral Device status
        /// </summary>
        [Description("Nonexistent status")]
        PeripheralDeviceStatusNonexistent
    }
}
