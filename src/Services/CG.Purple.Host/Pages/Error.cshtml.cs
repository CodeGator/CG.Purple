
namespace CG.Purple.Host.Pages
{
    /// <summary>
    /// This class is the code-behind for the Error page.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        // *******************************************************************
        // Fields.
        // *******************************************************************

        #region Fields

        /// <summary>
        /// This field contains the logger for this model.
        /// </summary>
        private readonly ILogger<ErrorModel> _logger;

        #endregion

        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains the request identifier for this model.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// This property indicates whether or not to show the <see cref="ErrorModel.RequestId"/>
        /// property on the error page.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        #endregion

        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="ErrorModel"/>
        /// class.
        /// </summary>
        /// <param name="logger">The logger for this model</param>
        /// <exception cref="ArgumentException">This exception is thrown whenever
        /// one or more arguments are missing, or empty.</exception>
        public ErrorModel(
            ILogger<ErrorModel> logger
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(logger, nameof(logger));

            // Save the reference(s).
            _logger = logger;
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method is called when the page receives an HTTP GET verb.
        /// </summary>
        public void OnGet()
        {
            // Pull the request id from the context.
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }

        #endregion
    }
}