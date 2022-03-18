using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bibliothèque;

namespace GstBdd
{
    public class GstBDD
    {
        private MySqlConnection cnx;
        private MySqlCommand cmd;
        private MySqlDataReader dr;

        public GstBDD()
        {
            string chaine = "Server=127.0.0.1;Database=gsb;Uid=root;Pwd=;SslMode=none";
            cnx = new MySqlConnection(chaine);
            cnx.Open();
        }

        public List<Medicament> GetAllMedicaments() // Oliver
        {   // Selectionne tous les medicaments et les transforment en objets dans la classe medicament via la "liste les Medicaments" de type medicament
            List<Medicament> LesMedicaments = new List<Medicament>();
            cmd = new MySqlCommand("SELECT MED_DEPOTLEGAL, MED_NOMCOMMERCIAL, FAM_COD, MED_COMPOSITION, MED_EFFETS, MED_CONTREINDIC, MED_PRIXECHANTILLON,FAM_LIBELLE  FROM medicament INNER JOIN Famille ON FAM_CODE = FAM_COD", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Famille codeF = new Famille(Convert.ToInt16(dr[2].ToString()), dr[7].ToString());
                Medicament uneNouveauMedicament = new Medicament(Convert.ToInt16(dr[0].ToString()), dr[1].ToString(), codeF, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), Convert.ToDouble(dr[6].ToString()));
                LesMedicaments.Add(uneNouveauMedicament);
            }
            dr.Close();
            return LesMedicaments;
        }

        public List<TypeIndividu> GetAllTypesIndividu() // Oliver
        {   // selectionne tous les types d'individu de la BDD
            List<TypeIndividu> lesTypesIndividu = new List<TypeIndividu>();
            cmd = new MySqlCommand("SELECT TIN_CODE, TIN_LIBELLE FROM type_individu", cnx);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                TypeIndividu unNouveauType = new TypeIndividu(Convert.ToInt16(dr[0].ToString()), dr[1].ToString());
                lesTypesIndividu.Add(unNouveauType);
            }
            dr.Close();
            return lesTypesIndividu;
        }

        public void AjoutPre(int unDepotLegal, int unCode, int doseCode, string unePosologie) // Oliver
        {   // Foncrtion qui insert une nouvelle prescription 
            cmd = new MySqlCommand("INSERT INTO prescrire (MED_DEPOTLEGAL, DOS_CODE, TIN_CODE, PRE_POSOLOGIE) VALUES (" + unDepotLegal + "," + unCode + "," + doseCode + ",'" + unePosologie + "'" + ")", cnx);
            cmd.ExecuteNonQuery();
        }

        public List<Dosage> GetAllDosage() // Oliver
        {   // Selectionne tous les dosages 
            List<Dosage> lesDoses = new List<Dosage>();
            cmd = new MySqlCommand("SELECT DOS_CODE, DOS_QUANTITE, DOS_UNITE FROM `dosage` ", cnx);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Dosage uneNouvelleDose = new Dosage(Convert.ToInt16(dr[0].ToString()), Convert.ToInt16(dr[1].ToString()), dr[2].ToString());
                lesDoses.Add(uneNouvelleDose);
            }
            dr.Close();
            return lesDoses;
        }

        public void UpdateTypeIndividu(string Libelle, int codeTin) // Oliver
        {   //met à jour le libelle d'un type individu en particulier 
            cmd = new MySqlCommand("UPDATE type_individu SET TIN_LIBELLE = " + "'" + Libelle + "' WHERE TIN_CODE = " + codeTin, cnx);
            cmd.ExecuteNonQuery();
        }

        public void AjouterTypeIndividu(string Libelle) // Oliver
        {   // insert un nouveau type d'individu
            cmd = new MySqlCommand("INSERT INTO type_individu VALUES (null,'" + Libelle + "')", cnx);
            cmd.ExecuteNonQuery();
        }


        public void ModifierMedicament(int id, string nom, int famille, string composition, string effets, string contreindic, double prix) // Romain 
        {   //Modifie le médicament sélectionné avec les nouvelles valeures sélectionnées
            cmd = new MySqlCommand("UPDATE medicament SET MED_NOMCOMMERCIAL = '" + nom + "', FAM_COD =" + famille + ", MED_COMPOSITION = '" + composition.Replace("'", "''") + "', MED_EFFETS = '" + effets.Replace("'", "''") + "', MED_CONTREINDIC = '" + contreindic.Replace("'", "''") + "', MED_PRIXECHANTILLON = " + prix + "  WHERE MED_DEPOTLEGAL = " + id, cnx);
            cmd.ExecuteNonQuery();
        }

        public List<Famille> GetAllFamilles() // Romain
        {    //affiche l'ID et le nom de toute les familles
            List<Famille> lesFamilles = new List<Famille>();
            cmd = new MySqlCommand("SELECT FAM_CODE, FAM_LIBELLE FROM famille", cnx);
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Famille uneFamille = new Famille(Convert.ToInt16(dr[0].ToString()), dr[1].ToString());
                lesFamilles.Add(uneFamille);
            }
            dr.Close();
            return lesFamilles;
        }

        public int GetIdFamille(string famille) // Romain
        {   //affiche l'ID de la famille sélectionnée
            cmd = new MySqlCommand("SELECT FAM_CODE FROM famille WHERE FAM_LIBELLE ='" + famille + "'", cnx);
            dr = cmd.ExecuteReader();
            dr.Read();
            int idFamille = Convert.ToInt16(dr[0].ToString());
            dr.Close();
            return idFamille;
        }

        
        public void AjoutMed(string nom, int famCode, decimal prix, string comp, string effet_med, string contre) // Quentin
        {   //GST qui ajoute un nouveaux médicament avec toute c'est information 
            double p = Convert.ToDouble(prix.ToString().Replace('.', '.'));
            cmd = new MySqlCommand("INSERT INTO medicament VALUES(null,'" + nom + "'," + famCode + ",'" + comp.Replace("'", "''") + "','" + effet_med.Replace("'", "''") + "','" + contre.Replace("'", "''") + "'," + p + ")", cnx);
            cmd.ExecuteNonQuery();
        }


         
        public List<Medicament> GetAllPertubateur(int num) // Quentin
        {   // Il permet d'avoir tout les pertubateur en fonction d'un medicament
            List<Medicament> LesPertubateur = new List<Medicament>();
            cmd = new MySqlCommand("SELECT MED_PERTURBATEUR, MED_NOMCOMMERCIAL, FAM_COD, MED_COMPOSITION, MED_EFFETS, MED_CONTREINDIC, MED_CONTREINDIC FROM medicament INNER JOIN interagir ON MED_DEPOTLEGAL = MED_PERTURBATEUR WHERE MED_MED_PERTURBE =" + num + " GROUP BY MED_PERTURBATEUR ", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Famille codeF = new Famille(Convert.ToInt16(dr[2].ToString()), "test");
                //Medicament inter = new Medicament(Convert.ToInt16(dr[0].ToString()), dr[1].ToString(), null, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), Convert.ToDouble(dr[6].ToString()));
                Medicament inter = new Medicament(Convert.ToInt16(dr[0].ToString()), dr[1].ToString(), null, null, null, null, 0);
                LesPertubateur.Add(inter);
            }
            dr.Close();
            return LesPertubateur;
        }

        
        public List<Medicament> GetAllNonPertubateur(int num) // Quentin
        {   //renvoyer tout les medicament qui ne sont pas pertubateur en fonction d'un medicament 
            List<Medicament> LesPertubateur = new List<Medicament>();
            cmd = new MySqlCommand("SELECT MED_DEPOTLEGAL, MED_NOMCOMMERCIAL FROM medicament INNER JOIN interagir ON MED_DEPOTLEGAL = MED_PERTURBATEUR WHERE MED_PERTURBATEUR  NOT IN(SELECT MED_PERTURBATEUR FROM medicament INNER JOIN interagir ON MED_DEPOTLEGAL = MED_PERTURBATEUR WHERE MED_MED_PERTURBE = " + num + ") AND MED_DEPOTLEGAL != " + num + " GROUP BY MED_DEPOTLEGAL", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Medicament inter = new Medicament(Convert.ToInt16(dr[0].ToString()), dr[1].ToString(), null, null, null, null, 0);
                LesPertubateur.Add(inter);
            }
            dr.Close();
            return LesPertubateur;
        }

        
        public void AjoutPertubateur(int pertubateur, int pertube) // Quentin
        {   //ajoute un pertubateur a un médicament donné
            cmd = new MySqlCommand("INSERT INTO interagir VALUES(" + pertubateur + "," + pertube + ")", cnx);
            cmd.ExecuteNonQuery();
        }
        
       
        public void DeletePertubateur(int pertubateur, int pertube) // Quentin
        {   //enleve un pertubateur d'un medicament donné
            cmd = new MySqlCommand("DELETE FROM interagir WHERE MED_PERTURBATEUR=" + pertubateur + " AND MED_MED_PERTURBE=" + pertube + ";", cnx);
            cmd.ExecuteNonQuery();
        }

        
        public Dictionary<string, int> GetDatasGraph1()// Romain
        {   //graphe des stat renvoie le prix échantillion par médicament 
            Dictionary<string, int> lesDatas2 = new Dictionary<string, int>(); 
            cmd = new MySqlCommand("SELECT medicament.MED_NOMCOMMERCIAL, medicament.MED_PRIXECHANTILLON FROM medicament", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lesDatas2.Add(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()));
            }
            dr.Close();
            return lesDatas2;
        }

        public Dictionary<string, int> GetDatasGraph2() // Oliver
        {   //renvoie le nombre de médicament par famille 
            Dictionary<string, int> lesDatas = new Dictionary<string, int>();
            cmd = new MySqlCommand("SELECT FAM_LIBELLE, COUNT(FAM_COD) FROM famille INNER JOIN medicament ON FAM_CODE = FAM_COD GROUP BY FAM_LIBELLE", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lesDatas.Add(dr[0].ToString(), Convert.ToInt32(dr[1].ToString()));
            }
            dr.Close();
            return lesDatas;
        }

        public Dictionary<string, double> GetDatasGraph3(int num) // Quentin
        {   //recupére les medicment pertubateur en fonction d'un médicament affiche le nom et le prix 
            Dictionary<string, double> lesDatas3 = new Dictionary<string, double>();
            cmd = new MySqlCommand("SELECT MED_NOMCOMMERCIAL,MED_PRIXECHANTILLON FROM medicament INNER JOIN interagir ON MED_DEPOTLEGAL = MED_PERTURBATEUR WHERE MED_PERTURBATEUR  NOT IN(SELECT MED_PERTURBATEUR FROM medicament INNER JOIN interagir ON MED_DEPOTLEGAL = MED_PERTURBATEUR WHERE MED_MED_PERTURBE = " + num + ") GROUP BY MED_DEPOTLEGAL;", cnx);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lesDatas3.Add(dr[0].ToString(), Convert.ToDouble(dr[1].ToString()));
            }
            dr.Close();
            return lesDatas3;
        }
    }
}

