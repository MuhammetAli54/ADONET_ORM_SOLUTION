using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADONET_ORM_BLL;
using ADONET_ORM_Entities.Entities;

namespace ADONET_ORM_FORMUI
{
    public partial class FormKitaplar : Form
    {
        public FormKitaplar()
        {
            InitializeComponent();
        }

        //Global Alan
        YazarlarORM myYazarORM = new YazarlarORM();
        TurlerORM myTurlerORM = new TurlerORM();
        KitaplarORM myKitapORM = new KitaplarORM();

        private void FormKitaplar_Load(object sender, EventArgs e)
        {
            TumYazarlariComboyaGetir();
            TumTurleriComboyaGetir();
            TumKitaplariGrideViewModelleGetir();
            TumKitaplariSilComboyaGetir();
            TumKtaplariGuncelleComboyaGetir();

            //comboBox'ların içine yazılamasın diye cmbox'ların style'larını düzenleyeceğiz.
            //1. Yöntem: comboların name'lerinden ilgili property'yi düzenlemektir.
            comboBoxKitapGuncelle.DropDownStyle = ComboBoxStyle.DropDownList;
            //2. Yöntem: foreach döngüsüyle form controlleri taranarak combo bulursa bulduğu nesnenin ilgili property'sini düzenlemektir.

            //UZUN YÖNTEM
            //foreach (var item in this.Controls)
            //{
            //    if (item is TabControl)
            //    {
            //        foreach (var subitem in ((TabControl)item).Controls)
            //        {
            //            if (subitem is TabPage)
            //            {
            //                foreach (var subofsubitem in ((TabPage)subitem).Controls)
            //                {
            //                    if (subofsubitem is ComboBox)
            //                    {
            //                        ((ComboBox)subofsubitem).DropDownStyle = ComboBoxStyle.DropDownList;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //Yukarıdaki foreach'e göre daha kısa yöntem
            //TabControl ---> TabPage
            for (int i = 0; i < this.Controls[0].Controls.Count; i++)
            {
                for (int k = 0; k < this.Controls[0].Controls[i].Controls.Count; k++)
                {
                    if (this.Controls[0].Controls[i].Controls[k] is ComboBox)
                    {
                        ((ComboBox)this.Controls[0].Controls[i].Controls[k]).DropDownStyle = ComboBoxStyle.DropDownList;
                    }
                }
            }
        }

        private void TumKtaplariGuncelleComboyaGetir()
        {
            comboBoxKitapGuncelle.DisplayMember = "KitapAdi";
            comboBoxKitapGuncelle.ValueMember = "KitapId";
            comboBoxKitapGuncelle.DataSource = myKitapORM.Select();
        }

        private void TumKitaplariSilComboyaGetir()
        {
            cmbBox_Sil_Kitap.DisplayMember = "KitapAdi";
            cmbBox_Sil_Kitap.ValueMember = "KitapId";

            // cmbBox_Sil_Kitap.DataSource = myKitapORM.Select();
            // Yukarıdaki gibi yapmak istemezsek yani
            // KitaplarORM class'ından instance almak istemezsek 
            // Class içine tanımladığımız static property aracılığıyla o instance'a ulaşmış oluruz
            // Aslında burada kendimize arka planda instance oluşturuyoruz ve static nesne aracılığıyla o nesneyi kullanıyoruz.
            //2. YOL
            cmbBox_Sil_Kitap.DataSource = KitaplarORM.Current.Select();

        }

        private void TumKitaplariGrideViewModelleGetir()
        {
            dataGridViewKitaplar.DataSource = myKitapORM.KitaplariViewModelleGetir();
            dataGridViewKitaplar.Columns["SilindiMi"].Visible = false;
            dataGridViewKitaplar.Columns["TurId"].Visible = false;
            dataGridViewKitaplar.Columns["YazarId"].Visible = false;
            for (int i = 0; i < dataGridViewKitaplar.Columns.Count; i++)
            {
                dataGridViewKitaplar.Columns[i].Width = 120;
            }

        }

        private void TumTurleriComboyaGetir()
        {
            cmbBox_Ekle_Tur.DisplayMember = "TurAdi";
            cmbBox_Ekle_Tur.ValueMember = "TurId";
            cmbBox_Ekle_Tur.DataSource = myTurlerORM.TurleriGetir();
            cmbBox_Ekle_Tur.SelectedIndex = 0;

            cmbBox_Guncelle_Tur.DisplayMember = "TurAdi";
            cmbBox_Guncelle_Tur.ValueMember = "TurId";
            cmbBox_Guncelle_Tur.DataSource = myTurlerORM.TurleriGetir();
        }

        private void TumYazarlariComboyaGetir()
        {
            cmbBox_Ekle_Yazar.DisplayMember = "YazarAdSoyad";
            cmbBox_Ekle_Yazar.ValueMember = "YazarId";
            cmbBox_Ekle_Yazar.DataSource = myYazarORM.Select();

            cmbBox_Guncelle_Yazar.DisplayMember = "YazarAdSoyad";
            cmbBox_Guncelle_Yazar.ValueMember = "YazarId";
            cmbBox_Guncelle_Yazar.DataSource = myYazarORM.Select();
        }

        private void btnKitapEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (numericUpDown_Ekle_SayfaSayisi.Value <= 0)
                {
                    MessageBox.Show("HATA: Sayfa sayısı sıfırdan büyük olmalıdır!");
                    return;
                }
                if (numericUpDown_Ekle_Stok.Value <= 0)
                {
                    MessageBox.Show("HATA: Stok sayısı sıfırdan büyük olmalıdır!");
                    return;
                }
                if ((int)cmbBox_Ekle_Yazar.SelectedValue <= 0)
                {
                    MessageBox.Show("HATA: Kitabın bir yazarı olmalıdır! Yazar seçiniz!");
                    return;
                }
                Kitap yeniKitap = new Kitap()
                {
                    KayitTarihi = DateTime.Now,
                    KitapAdi = txtKitapEkle.Text.Trim(),
                    SayfaSayisi = (int)numericUpDown_Ekle_SayfaSayisi.Value,
                    Stok = (int)numericUpDown_Ekle_Stok.Value,
                    SilindiMi = false,
                    YazarId = (int)cmbBox_Ekle_Yazar.SelectedValue
                };

                //TurId null mı değil mi?
                if ((int)cmbBox_Ekle_Tur.SelectedValue == -1)  //Tür yok'u seçmiş
                {
                    yeniKitap.TurId = null;
                }
                else
                {
                    yeniKitap.TurId = (int)cmbBox_Ekle_Tur.SelectedValue;
                }
                if (myKitapORM.Insert(yeniKitap))
                {
                    MessageBox.Show($"{yeniKitap.KitapAdi} kitabı yeni kitap olarak eklendi...");
                    TumKitaplariGrideViewModelleGetir();
                    //Temizlik
                    EkleSayfasiKontrolleriTemizle();
                    TumKtaplariGuncelleComboyaGetir();
                    TumKitaplariSilComboyaGetir();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA: " + ex.Message);
            }
        }

        private void EkleSayfasiKontrolleriTemizle()
        {
            txtKitapEkle.Clear();
            cmbBox_Ekle_Tur.SelectedIndex = -1;
            cmbBox_Ekle_Yazar.SelectedIndex = -1;
            numericUpDown_Ekle_SayfaSayisi.Value = 0;
            numericUpDown_Ekle_Stok.Value = 0;
        }

        private void btnKitapSil_Click(object sender, EventArgs e)
        {
            try
            {
                if ((int)cmbBox_Sil_Kitap.SelectedValue<=0)
                {
                    MessageBox.Show("Lütfen kitap seçimi yapınız!", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                Kitap kitabim = myKitapORM.SelectET((int)cmbBox_Sil_Kitap.SelectedValue);

                DialogResult cevap = MessageBox.Show($"Bu kitabı silmek yerine pasifleştirmek ister misiniz ? \n" +
                    $"Pasifleştirmek için ---> Evet'e tıklayınız \n" +
                    $"Silmek için ---> Hayır'a tıklayınız", "SİLME ONAY", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (cevap == DialogResult.Yes)
                {
                    //pasifleştirme işlemi update ile yapılmalı
                    kitabim.SilindiMi = true;
                    switch (myKitapORM.Update(kitabim))
                    {
                        case true:
                            MessageBox.Show($"{kitabim.KitapAdi} sistemde pasifleştirildi");
                            SilSayfasiTemizle();
                            break;
                        case false:
                            throw new Exception($"HATA: {kitabim.KitapAdi} pasifleştirme işleminde beklenmedik bir hata oldu!");
                    }


                   
                }
                else if (cevap == DialogResult.No)
                {
                    //silme
                   
                    //linq yazdık
                    var oduncListe = OduncIslemlerORM.Current.Select().Where(x => x.KitapId == kitabim.KitapId).ToList();
                    if (oduncListe.Count > 0)
                    {
                        MessageBox.Show("DİKKAT: Bu kitap ödünç alınmıştır! Silinemez...!", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    //Yukarıdaki if'e girmezse return olmaz
                    //Return olmazsa kod aşağı doğru okumaya devam eder.
                   
                    if (myKitapORM.Delete(kitabim))
                    {
                        MessageBox.Show($"{kitabim.KitapAdi} adlı kitap silinmiştir");

                        //temizlik
                        SilSayfasiTemizle();
                        TumKitaplariSilComboyaGetir();
                        TumKtaplariGuncelleComboyaGetir();
                        TumKitaplariGrideViewModelleGetir();
                    }
                    else
                    {
                        throw new Exception($"{kitabim.KitapAdi} silinememiştir!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA: Silme işleminde beklenmedik bir hata oldu!" + ex.Message);
            }
        }

        private void SilSayfasiTemizle()
        {
            TumKitaplariSilComboyaGetir();          //DON'T REPEAT YOURSELF (DRY)
            cmbBox_Sil_Kitap.SelectedIndex = -1;
            richTextBoxKitap.Clear();
        }

        private void comboBoxKitapGuncelle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GuncelleSayfasiTemizle();
                if (comboBoxKitapGuncelle.SelectedIndex>=0)
                {
                    Kitap secilenKitap = myKitapORM.SelectET((int)comboBoxKitapGuncelle.SelectedValue);
                    txt_GuncelleKitapAdi.Text = secilenKitap.KitapAdi;
                    numericUpDown_Guncelle_SayfaSayisi.Value = secilenKitap.SayfaSayisi;
                    numericUpDown_Guncelle_Stok.Value = secilenKitap.Stok;

                    if (secilenKitap.TurId==null)
                    {
                      //cmbBox_Guncelle_Tur.SelectedIndex = 0;
                      cmbBox_Guncelle_Tur.SelectedValue = -1;
                    }
                    else
                    {
                        cmbBox_Guncelle_Tur.SelectedValue = secilenKitap.TurId;
                    }

                    cmbBox_Guncelle_Yazar.SelectedValue = secilenKitap.YazarId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA: " + ex.Message);
            }
        }

        private void GuncelleSayfasiTemizle()
        {
            txt_GuncelleKitapAdi.Text = string.Empty;
            numericUpDown_Guncelle_SayfaSayisi.Value = 0;
            numericUpDown_Guncelle_Stok.Value = 0;
            cmbBox_Guncelle_Tur.SelectedIndex = -1;
            cmbBox_Guncelle_Yazar.SelectedIndex = -1;
        }

        private void btnKitapGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxKitapGuncelle.SelectedIndex >= 0)
                {
                    if (numericUpDown_Guncelle_SayfaSayisi.Value <= 0)
                    {
                        throw new Exception("HATA : Sayfa sayısı sıfırdan büyük olmalıdır!");
                    }
                    if (numericUpDown_Guncelle_Stok.Value <= 0)
                    {
                        throw new Exception("HATA: Kitap stoğu sıfırdan büyük olmalıdır!");
                    }
                    Kitap secilenKitap = myKitapORM.SelectET((int)comboBoxKitapGuncelle.SelectedValue);
                    if (secilenKitap == null)
                    {
                        throw new Exception("HATA: Kitap bulunamadı!");
                    }
                    else
                    {
                        secilenKitap.KitapAdi = txt_GuncelleKitapAdi.Text.Trim();
                        secilenKitap.SayfaSayisi = (int)numericUpDown_Guncelle_SayfaSayisi.Value;
                        secilenKitap.Stok = (int)numericUpDown_Guncelle_Stok.Value;
                        secilenKitap.SilindiMi = false;
                        secilenKitap.YazarId = (int)cmbBox_Guncelle_Yazar.SelectedValue;
                        if ((int)cmbBox_Guncelle_Tur.SelectedValue == -1)    //Türü  yok
                        {
                            secilenKitap.TurId = null;
                        }
                        else
                        {
                            secilenKitap.TurId = (int)cmbBox_Guncelle_Tur.SelectedValue;
                        }
                        switch (myKitapORM.Update(secilenKitap))
                        {
                            case true:
                                MessageBox.Show($"{secilenKitap.KitapAdi} başarıyla güncellendi...");
                                //temizlik
                                TumKtaplariGuncelleComboyaGetir();
                                TumKitaplariSilComboyaGetir();
                                TumKitaplariGrideViewModelleGetir();
                                break;
                            case false:
                                throw new Exception($"HATA: {secilenKitap.KitapAdi} güncellenirken bir hata oluştu!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA: " + ex.Message);
            }
        }

        private void cmbBox_Sil_Kitap_SelectedIndexChanged(object sender, EventArgs e)
        {
            //richtextbox dolmalı
            if (cmbBox_Sil_Kitap.SelectedIndex>=0)
            {
                Kitap secilenKitap = myKitapORM.SelectET((int)cmbBox_Sil_Kitap.SelectedValue);

                if (secilenKitap!=null)
                {
                    string turu = secilenKitap.TurId == null ? "Türü Yok" : myTurlerORM.Select().FirstOrDefault(x => x.TurId == secilenKitap.TurId).TurAdi;

                    richTextBoxKitap.Text = $"Kitap: {secilenKitap.KitapAdi} \n \n" +
                        $"Türü: {turu} \n \n" +
                        $"Yazarı: {myYazarORM.Select().FirstOrDefault(x=> x.YazarId==secilenKitap.YazarId).YazarAdSoyad} \n \n" +
                        $"Sayfa Sayısı: {secilenKitap.SayfaSayisi} \n \n" +
                        $"Stok Adeti: {secilenKitap.Stok} adet stokta var";
                }
                else
                {

                }
            }

        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            //tablar değiştikçe temizlik yapıls
            comboBoxKitapGuncelle.SelectedIndex = -1;
            GuncelleSayfasiTemizle();
            SilSayfasiTemizle();
        }
    }
}
