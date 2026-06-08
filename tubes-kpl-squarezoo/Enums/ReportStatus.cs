namespace tubes_kpl_squarezoo.Enums
{
    /// <summary>
    /// MVP report status. 0 = Diterima, 1 = Diproses, 2 = Selesai, 3 = Ditolak.
    /// </summary>
    public enum ReportStatus
    {
        /// <summary>
        /// Report has been received.
        /// </summary>
        Diterima = 0,

        /// <summary>
        /// Report is being processed.
        /// </summary>
        Diproses = 1,

        /// <summary>
        /// Report has been completed.
        /// </summary>
        Selesai = 2,

        /// <summary>
        /// Report has been rejected.
        /// </summary>
        Ditolak = 3
    }
}
