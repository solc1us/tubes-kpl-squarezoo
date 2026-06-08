namespace tubes_kpl_squarezoo.Enums
{
    /// <summary>
    /// Text-based evidence type. 0 = Testimoni, 1 = KronologiTambahan, 2 = CatatanPendukung.
    /// </summary>
    public enum EvidenceType
    {
        /// <summary>
        /// Witness or victim testimony.
        /// </summary>
        Testimoni = 0,

        /// <summary>
        /// Additional chronology text.
        /// </summary>
        KronologiTambahan = 1,

        /// <summary>
        /// Supporting note text.
        /// </summary>
        CatatanPendukung = 2
    }
}
