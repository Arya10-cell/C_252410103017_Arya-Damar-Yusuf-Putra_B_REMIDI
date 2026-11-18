using System;
using System.Collections.Generic;

//1. 
public class Agen
{
    public string IdAgen { get; set; }
    public string NamaAgen { get; set; }
    public int StokAgen { get; private set; }

    public Agen(string idAgen, string namaAgen, int stokAgen)
    {
        IdAgen = idAgen;
        NamaAgen = namaAgen;
        StokAgen = stokAgen;
    }

    public void TampilInfo()
    {
        Console.WriteLine($"ID: {IdAgen} | Nama: {NamaAgen} | Stok: {StokAgen}");
    }

    public void TambahStok(int jumlah)
    {
        if (jumlah > 0)
            StokAgen += jumlah;
    }
}

class Program
{
    static void Main()
    {
        Agen agen1 = new Agen("AG-001", "Markas Tunjungan", 50);
        Agen agen2 = new Agen("AG-002", "Cabang Gubeng", 30);

        agen1.TampilInfo();
        agen2.TampilInfo();
    }
}

//2.
using System;

public class ProdukGalon
{
    private string idProduk;
    private string namaProduk;
    private int stokGudang;

    public ProdukGalon(string idProduk, string namaProduk, int stokAwal)
    {
        this.idProduk = idProduk;
        this.namaProduk = namaProduk;
        
        this.stokGudang = stokAwal >= 0 ? stokAwal : 0;
    }

    public string IdProduk
    {
        get { return idProduk; }
        set { idProduk = value; }
    }

    public string NamaProduk
    {
        get { return namaProduk; }
        set { namaProduk = value; }
    }

    
    public int StokGudang
    {
        get { return stokGudang; }
    }

    
    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0 || jumlah > stokGudang)
            return false;

        stokGudang -= jumlah;
        return true;
    }
}

//3.
using System;

public class GalonAkua : ProdukGalon
{
    public GalonAkua(string idProduk, string namaProduk, int stokAwal)
        : base(idProduk, namaProduk, stokAwal)
    {
    }
}

public class GalonBerkeran : ProdukGalon
{
    public string JenisKeran { get; set; }

    public GalonBerkeran(string idProduk, string namaProduk, int stokAwal, string jenisKeran)
        : base(idProduk, namaProduk, stokAwal)
    {
        JenisKeran = jenisKeran;
    }
}

//4.
using System;

public class ProdukGalon
{
    

    public virtual int HitungBiaya(int jumlah)
    {
        return 0;
    }
}

public class GalonAkua : ProdukGalon
{
    public GalonAkua(string idProduk, string namaProduk, int stokAwal)
        : base(idProduk, namaProduk, stokAwal) { }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * 20000;
    }
}

public class GalonBerkeran : ProdukGalon
{
    public string JenisKeran { get; set; }

    public GalonBerkeran(string idProduk, string namaProduk, int stokAwal, string jenisKeran)
        : base(idProduk, namaProduk, stokAwal)
    {
        JenisKeran = jenisKeran;
    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * 12000;
    }
}

class Program
{
    static void Main()
    {
        
        ProdukGalon[] daftarProduk =
        {
            new GalonAkua("P-01", "Akua Murni", 100),
            new GalonBerkeran("P-02", "Berkeran Impor", 80, "Keran Logam")
        };

        foreach (ProdukGalon produk in daftarProduk)
        {
            
            Console.WriteLine($"{produk.NamaProduk}: Rp{produk.HitungBiaya(5)} untuk 5 galon");
        }
    }
}

//5.
using System;


public abstract class ProdukGalon
{
   
    public abstract int HitungBiaya(int jumlah);
}

//6.
using System;

public interface DapatDidistribusikan
{
    bool KirimPaket();
}

public class DistribusiGalon : DapatDidistribusikan
{
    public Agen AgenTujuan { get; set; }
    public ProdukGalon Produk { get; set; }
    public int Jumlah { get; set; }

    public DistribusiGalon(Agen agenTujuan, ProdukGalon produk, int jumlah)
    {
        AgenTujuan = agenTujuan;
        Produk = produk;
        Jumlah = jumlah;
    }

    public bool KirimPaket()
    {
        
        if (Jumlah <= 0)
        {
            Console.WriteLine("GAGAL: Jumlah galon harus lebih dari 0.");
            return false;
        }

        
        if (Jumlah > Produk.StokGudang)
        {
            Console.WriteLine($"GAGAL: Stok gudang tidak cukup. Sisa stok {Produk.NamaProduk}: {Produk.StokGudang} galon.");
            return false;
        }

        
        Produk.KurangiStok(Jumlah);       
        AgenTujuan.TambahStok(Jumlah);    
        Console.WriteLine($"Paket berhasil dikirimkan ke agen {AgenTujuan.NamaAgen}: {Jumlah} galon {Produk.NamaProduk}.");
        return true;
    }
}

class Program
{
    static void Main()
    {
        Agen agen = new Agen("AG-001", "Markas Tunjungan", 50);
        ProdukGalon produk = new GalonAkua("P-01", "Akua Murni", 100);

        DapatDidistribusikan kiriman = new DistribusiGalon(agen, produk, 20);
        kiriman.KirimPaket();   

        DapatDidistribusikan kirimanGagal = new DistribusiGalon(agen, produk, 500);
        kirimanGagal.KirimPaket();  
    }
}

//7.
using System;
using System.Collections.Generic;

public class SindikatGalon
{
    private List<Agen> daftarAgen = new List<Agen>();

    
    private List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

    public void TambahAgen(Agen agen)
    {
        daftarAgen.Add(agen);
        Console.WriteLine($"Agen baru direkrut: {agen.NamaAgen}");
    }

    public void TambahProduk(ProdukGalon produk)
    {
        daftarProduk.Add(produk);
        Console.WriteLine($"Pasokan galon ditambahkan ke gudang pusat: {produk.NamaProduk}");
    }

    public void TampilkanSindikat()
    {
        Console.WriteLine("=== DATA AGEN ===");
        foreach (Agen agen in daftarAgen)
        {
            agen.TampilInfo();
        }

        Console.WriteLine("\n=== DATA PRODUK GALON ===");
        foreach (ProdukGalon produk in daftarProduk)
        {
            Console.WriteLine($"ID: {produk.IdProduk} | Nama: {produk.NamaProduk} | Stok: {produk.StokGudang}");
        }
    }
}

class Program
{
    static void Main()
    {
        SindikatGalon sindikat = new SindikatGalon();

        sindikat.TambahAgen(new Agen("AG-001", "Markas Tunjungan", 50));
        sindikat.TambahAgen(new Agen("AG-002", "Cabang Gubeng", 30));

        sindikat.TambahProduk(new GalonAkua("P-01", "Akua Murni", 100));
        sindikat.TambahProduk(new GalonBerkeran("P-02", "Berkeran Impor", 80, "Keran Logam"));

        Console.WriteLine();
        sindikat.TampilkanSindikat();
    }
}