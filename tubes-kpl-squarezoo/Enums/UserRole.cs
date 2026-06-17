namespace tubes_kpl_squarezoo.Enums
{
    /// <summary>
    /// Lapor-U user role. 0 = Pelapor, 1 = Admin, 2 = Pimpinan.
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// Reporter who can create and track reports.
        /// </summary>
        Pelapor = 0,

        /// <summary>
        /// Admin or Satgas user who can manage reports.
        /// </summary>
        Admin = 1,

        /// <summary>
        /// Leadership user who can view summary data.
        /// </summary>
        Pimpinan = 2
    }
}
