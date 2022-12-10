namespace CG.Purple.Tools.TestClient
{
    /// <summary>
    /// This class is the code-behind for the <see cref="App"/> application.
    /// </summary>
    public partial class App : Application
    {
        // *******************************************************************
        // Constructors.
        // *******************************************************************

        #region Constructors

        /// <summary>
        /// This constructor creates a new instance of the <see cref="App"/>
        /// class.
        /// </summary>
        public App()
        {
            // Make MAUI happy.
            InitializeComponent();

            // Create the main window.
            MainPage = new MainPage();
        }

        #endregion

        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method is called to create the application window.
        /// </summary>
        /// <param name="activationState">The activation state to use for the
        /// operation.</param>
        /// <returns>The main window for the application.</returns>
        protected override Window CreateWindow(
            IActivationState? activationState
            )
        {
            // Give the base class a chance.
            var window = base.CreateWindow(
                activationState
                );

            // Change the startup size.
            window.Width = 800;
            window.Height = 1200;

            // Return the results.
            return window;
        }

        #endregion
    }
}