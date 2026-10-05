using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchaeologicalTimeMachine.Domain.Entities;
using ArchaeologicalTimeMachine.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ArchaeologicalTimeMachine.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ArchaeologyDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Sites.AnyAsync())
        {
            return; // Database already seeded
        }

        // ==========================================
        // 1. CIVILIZATIONS & CULTURAL TRADITIONS
        // ==========================================
        var harappan = new Civilization
        {
            Name = "Indus Valley (Harappan) Civilization",
            Slug = "indus-valley-harappan",
            Region = "South Asia (Indus, Ghaggar-Hakra & Gujarat)",
            StartYear = -3300,
            EndYear = -1300,
            ColorHex = "#E06A3B",
            PrimaryLanguage = "Undeciphered Indus Script",
            ArchitecturalTradition = "Standardized fired mudbrick, orthogonal grid planning, monumental stepwells and subterranean covered drains",
            Description = "One of the three earliest cradles of Old World civilization, distinguished by remarkable urban planning, standardized weights, maritime trading networks, and advanced hydraulic engineering without evidence of monarchical despotism."
        };

        var maurya = new Civilization
        {
            Name = "Mauryan Empire & Early Historic India",
            Slug = "mauryan-early-historic",
            Region = "South Asia (Pan-Indian Subcontinent)",
            StartYear = -322,
            EndYear = -185,
            ColorHex = "#D90429",
            PrimaryLanguage = "Prakrit & Sanskrit (Brahmi & Kharosthi Scripts)",
            ArchitecturalTradition = "Polished Chunar sandstone monolithic pillars, rock-cut chaityas, stupas, and monumental hypostyle halls",
            Description = "The first pan-Indian empire founded by Chandragupta Maurya and reached its philosophical and cultural zenith under Emperor Ashoka, renowned for the Edicts of Ashoka, Buddhist patronage, and monumental stone art."
        };

        var sangam = new Civilization
        {
            Name = "Sangam Age & Tamil Maritime Sphere",
            Slug = "sangam-tamil-maritime",
            Region = "South India (Tamilakam) & Indian Ocean",
            StartYear = -600,
            EndYear = 300,
            ColorHex = "#2B9348",
            PrimaryLanguage = "Old Tamil (Tamil-Brahmi Script)",
            ArchitecturalTradition = "Brick residential quarters, ring wells, industrial dye vats, port wharves, and Megalithic cist burials",
            Description = "A highly literate, urban, and seafaring civilization of deep South India celebrated for classical Sangam literature, gem manufacturing, and extensive maritime trade with the Roman Empire and Southeast Asia."
        };

        var copperHoard = new Civilization
        {
            Name = "Indian Copper Hoard & Warrior Horizon",
            Slug = "indian-copper-hoard",
            Region = "South Asia (Ganga-Yamuna Doab & Central India)",
            StartYear = -2000,
            EndYear = -1400,
            ColorHex = "#B5838D",
            PrimaryLanguage = "Unrecorded (Pre-Vedic / Proto-Indo-Aryan)",
            ArchitecturalTradition = "Mudbrick fortified encampments, decorated wooden royal coffins, and copper metallurgical workshops",
            Description = "A formidable martial and metallurgical culture of the 2nd millennium BCE Ganga-Yamuna doab, characterized by solid-wheeled war chariots, copper helmets, and anthropomorphic bronze weaponry."
        };

        var prehistoricIndia = new Civilization
        {
            Name = "Prehistoric Rock Art & Mesolithic India",
            Slug = "prehistoric-mesolithic-india",
            Region = "South Asia (Vindhyas, Central India & Deccan)",
            StartYear = -100000,
            EndYear = -1000,
            ColorHex = "#6D597A",
            PrimaryLanguage = "Pre-linguistic / Oral Symbolic",
            ArchitecturalTradition = "Natural sandstone rock shelters, overhang habitations, and cave wall galleries",
            Description = "Deep-time human cultural continuum spanning the Acheulian, Middle Paleolithic, and Mesolithic hunter-gatherer eras, producing the world's densest concentration of prehistoric rock art."
        };

        var mesopotamian = new Civilization
        {
            Name = "Mesopotamian Civilizations (Sumer, Akkad, Babylon)",
            Slug = "mesopotamian-civilizations",
            Region = "Near East (Tigris-Euphrates Basin)",
            StartYear = -4500,
            EndYear = -539,
            ColorHex = "#D4A373",
            PrimaryLanguage = "Sumerian & Akkadian (Cuneiform)",
            ArchitecturalTradition = "Mudbrick monumental ziggurats, buttressed temple precincts, and glazed polychrome processional gates",
            Description = "The fertile crescent society pioneered cuneiform writing, sexagesimal mathematics, the wheel, codified law codes (Code of Hammurabi), and massive step-pyramidal ziggurats."
        };

        var ancientEgyptian = new Civilization
        {
            Name = "Ancient Egyptian Civilization",
            Slug = "ancient-egyptian",
            Region = "Northeast Africa (Nile Valley)",
            StartYear = -3100,
            EndYear = -30,
            ColorHex = "#E9C46A",
            PrimaryLanguage = "Ancient Egyptian (Hieroglyphic & Hieratic)",
            ArchitecturalTradition = "Ashlar limestone and granite monumental pyramids, hypostyle halls, pylon gateways, and rock-cut royal hypogea",
            Description = "Nile-centric civilization famous for monumental stone architecture, divine kingship, sophisticated mortuary theology, and continuous cultural longevity spanning three millennia."
        };

        var minoan = new Civilization
        {
            Name = "Minoan Civilization",
            Slug = "minoan",
            Region = "Aegean (Crete)",
            StartYear = -2700,
            EndYear = -1100,
            ColorHex = "#2A9D8F",
            PrimaryLanguage = "Eteocretan / Linear A (Undeciphered)",
            ArchitecturalTradition = "Multi-story labyrinthine palaces around central courts, lightwells, ashlar masonry, and vivid maritime fresco decorations",
            Description = "Bronze Age Aegean maritime civilization centered on Crete, celebrated for unfortified palatial centers, maritime thalassocracy, bull-leaping rituals, and sophisticated plumbing."
        };

        var roman = new Civilization
        {
            Name = "Roman Civilization",
            Slug = "roman",
            Region = "Mediterranean Basin & Europe",
            StartYear = -753,
            EndYear = 476,
            ColorHex = "#9B2226",
            PrimaryLanguage = "Latin",
            ArchitecturalTradition = "Hydraulic pozzolanic concrete, triumphal arches, amphitheaters, basilica vaults, and long-range aqueducts",
            Description = "Classical Mediterranean empire that forged standardized civil law, urban planning, extensive paved road networks, and engineering marvels utilizing the arch, dome, and concrete."
        };

        var kushite = new Civilization
        {
            Name = "Kingdom of Kush (Meroitic Civilization)",
            Slug = "kingdom-of-kush-meroe",
            Region = "Northeast Africa (Nubia / Sudan)",
            StartYear = -1070,
            EndYear = 350,
            ColorHex = "#C59B27",
            PrimaryLanguage = "Meroitic (Undeciphered Hieroglyphic & Cursive Scripts)",
            ArchitecturalTradition = "Steep-angled sandstone Nubian pyramids, royal mortuary chapels, pylon entrances, and blast furnace ironworks",
            Description = "The formidable Nubian empire of Kush that ruled Egypt as the 25th Dynasty and later transferred its capital south to Meroë, renowned for royal pyramids, matriarchal warrior queens (Candaces), and early African iron-smelting."
        };

        var nabataean = new Civilization
        {
            Name = "Nabataean Civilization",
            Slug = "nabataean-civilization",
            Region = "Near East (Levant / Arabia Petraea)",
            StartYear = -400,
            EndYear = 106,
            ColorHex = "#E76F51",
            PrimaryLanguage = "Nabataean Aramaic & Early Arabic",
            ArchitecturalTradition = "Monolithic rock-cut facades with Hellenistic pediments, cliff-hewn tombs, and pressurized ceramic water piping",
            Description = "Master desert nomads and merchants who controlled the ancient frankincense and spice caravan routes between Arabia, India, and the Mediterranean, establishing their rock-cut capital at Petra."
        };

        var inca = new Civilization
        {
            Name = "Inca Civilization (Tawantinsuyu)",
            Slug = "inca-civilization",
            Region = "South America (Andes / Peru)",
            StartYear = 1200,
            EndYear = 1572,
            ColorHex = "#F4A261",
            PrimaryLanguage = "Quechua",
            ArchitecturalTradition = "Cyclopean dry-stone ashlar masonry, seismic-resistant trapezoidal doorways, agricultural terraces, and royal estates",
            Description = "The largest pre-Columbian empire in the Americas, renowned for monumental mountain citadels like Machu Picchu, an extensive 40,000 km road network (Qhapaq Ñan), and quipu record-keeping."
        };

        var megalithic = new Civilization
        {
            Name = "Atlantic Megalithic & British Bronze Age",
            Slug = "atlantic-megalithic",
            Region = "Western Europe (Britain & Ireland)",
            StartYear = -4000,
            EndYear = -1000,
            ColorHex = "#457B9D",
            PrimaryLanguage = "Pre-Indo-European / Insular Celtic",
            ArchitecturalTradition = "Trilithon post-and-lintel stone circles, henges, round barrows, and solstitial alignments",
            Description = "Neolithic and Bronze Age societies of the Atlantic facade who erected monumental stone circles, earthworks, and astronomically aligned megaliths across Salisbury Plain and Avebury."
        };

        var khmer = new Civilization
        {
            Name = "Khmer Empire (Angkorian Civilization)",
            Slug = "khmer-empire",
            Region = "Southeast Asia (Cambodia)",
            StartYear = 802,
            EndYear = 1431,
            ColorHex = "#2A9D8F",
            PrimaryLanguage = "Khmer & Sanskrit",
            ArchitecturalTradition = "Sandstone and laterite temple-mountains representing Mount Meru, cruciform galleries, and vast hydraulic reservoirs",
            Description = "A magnificent Southeast Asian empire renowned for supreme hydraulic engineering, vast artificial lakes (Barays), and the world's largest religious sanctuary at Angkor Wat."
        };

        context.Civilizations.AddRange(harappan, maurya, sangam, copperHoard, prehistoricIndia, mesopotamian, ancientEgyptian, minoan, roman, kushite, nabataean, inca, megalithic, khmer);
        await context.SaveChangesAsync();

        // ==========================================
        // 2. HISTORICAL PERIODS & EPOCHS
        // ==========================================
        var rockArtEpoch = new HistoricalPeriod
        {
            Name = "Paleolithic to Mesolithic Rock Art Horizon",
            Slug = "paleolithic-mesolithic-rock-art",
            Epoch = "Stone Age",
            StartYear = -100000,
            EndYear = -3300,
            Description = "Deep hunter-gatherer epoch defined by microlithic toolkits and rich pictorial cave art expressing ritual and ecology."
        };

        var earlyBronze = new HistoricalPeriod
        {
            Name = "Early Bronze Age / Early Urban Horizon",
            Slug = "early-bronze-age",
            Epoch = "Bronze Age",
            StartYear = -3300,
            EndYear = -2100,
            Description = "Formative urbanization period marked by the rise of city-states, early writing systems, metallurgy, and monumental tomb construction."
        };

        var matureHarappanPeriod = new HistoricalPeriod
        {
            Name = "Mature Harappan Period (Integration Era)",
            Slug = "mature-harappan",
            Epoch = "Bronze Age",
            StartYear = -2600,
            EndYear = -1900,
            Description = "Apex of Indus urbanization: uniform orthogonal city layouts, standardized weights, seals, baked brick architecture, and international trade."
        };

        var lateHarappanCopperAge = new HistoricalPeriod
        {
            Name = "Late Harappan & Copper Hoard Warrior Era",
            Slug = "late-harappan-copper-age",
            Epoch = "Bronze/Copper Age",
            StartYear = -2000,
            EndYear = -1400,
            Description = "Decentralization of Indus cities coupled with the flourishing of Copper Hoard warrior complexes, war chariots, and localized agrarian cultures."
        };

        var secondUrbanizationPeriod = new HistoricalPeriod
        {
            Name = "Northern Black Polished Ware & Second Urbanization",
            Slug = "nbpw-second-urbanization",
            Epoch = "Iron Age",
            StartYear = -700,
            EndYear = -300,
            Description = "Re-emergence of large cities in the Gangetic basin, rise of the 16 Mahajanapadas, early coinage (punch-marked), and philosophical movements (Buddhism, Jainism)."
        };

        var mauryanImperialPeriod = new HistoricalPeriod
        {
            Name = "Mauryan Imperial Era",
            Slug = "mauryan-imperial-era",
            Epoch = "Antiquity",
            StartYear = -322,
            EndYear = -185,
            Description = "Unification of the Indian subcontinent under the Mauryas, stone edict diplomacy, royal highways, and monumental architecture."
        };

        var sangamAndIndoRomanPeriod = new HistoricalPeriod
        {
            Name = "Sangam Era & Indo-Roman Maritime Horizon",
            Slug = "sangam-indo-roman-horizon",
            Epoch = "Antiquity",
            StartYear = -600,
            EndYear = 300,
            Description = "Flourishing of early historic South Indian kingdoms (Chera, Chola, Pandya) linked to trans-oceanic spice and gemstone commerce with Rome."
        };

        var classicalAntiquity = new HistoricalPeriod
        {
            Name = "Classical Antiquity",
            Slug = "classical-antiquity",
            Epoch = "Iron Age & Antiquity",
            StartYear = -600,
            EndYear = 476,
            Description = "Era of Greek city-states, the Persian Empire, Hellenistic kingdoms, and the Roman Republic and Empire."
        };

        var postClassicalHorizon = new HistoricalPeriod
        {
            Name = "Post-Classical & Medieval World Horizons",
            Slug = "post-classical-medieval",
            Epoch = "Post-Classical",
            StartYear = 500,
            EndYear = 1600,
            Description = "Flourishing of monumental hydraulic states, medieval empires, and high civilizational monuments across Southeast Asia and the Americas (Angkor Wat, Khmer Empire, Tawantinsuyu Inca)."
        };

        context.HistoricalPeriods.AddRange(rockArtEpoch, earlyBronze, matureHarappanPeriod, lateHarappanCopperAge, secondUrbanizationPeriod, mauryanImperialPeriod, sangamAndIndoRomanPeriod, classicalAntiquity, postClassicalHorizon);
        await context.SaveChangesAsync();

        // ==========================================
        // 3. ACADEMIC REFERENCES & BIBLIOGRAPHY
        // ==========================================
        var refBisht = new Reference
        {
            CitationKey = "Bisht2015",
            Authors = "Bisht, Ravindra Singh",
            PublicationYear = 2015,
            Title = "Excavations at Dholavira (1990-2005)",
            JournalOrPublisher = "Archaeological Survey of India (New Delhi)",
            Url = "https://asi.nic.in"
        };

        var refRao = new Reference
        {
            CitationKey = "Rao1979",
            Authors = "Rao, Shikaripura Ranganatha",
            PublicationYear = 1979,
            Title = "Lothal: A Harappan Port Town (1955-1962), Vol. I & II",
            JournalOrPublisher = "Memoirs of the Archaeological Survey of India, No. 78",
            Url = "https://asi.nic.in"
        };

        var refShinde = new Reference
        {
            CitationKey = "Shinde2019",
            Authors = "Shinde, Vasant; Narasimhan, V. M.; Reich, David et al.",
            PublicationYear = 2019,
            Title = "An Ancient Harappan Genome Lacks Ancestry from Steppe Pastoralists or Iranian Farmers",
            JournalOrPublisher = "Cell, 179(3), pp. 729-735",
            DoiOrIsbn = "10.1016/j.cell.2019.08.048"
        };

        var refLal = new Reference
        {
            CitationKey = "Lal2003",
            Authors = "Lal, Braj Basi; Thapar, B. K.; Joshi, J. P.",
            PublicationYear = 2003,
            Title = "Excavations at Kalibangan: The Early Harappans (1960-1969)",
            JournalOrPublisher = "Archaeological Survey of India (New Delhi)"
        };

        var refManjul = new Reference
        {
            CitationKey = "Manjul2020",
            Authors = "Manjul, Sanjay Kumar; Manjul, Arvin",
            PublicationYear = 2020,
            Title = "Archaeological Excavations at Sinauli: Unearthing the Warrior Culture of 2000 BCE",
            JournalOrPublisher = "Puratattva: Journal of the Indian Archaeological Society, No. 50"
        };

        var refWakankar = new Reference
        {
            CitationKey = "Wakankar1976",
            Authors = "Wakankar, Vishnu Shridhar; Brooks, Robert R. R.",
            PublicationYear = 1976,
            Title = "Stone Age Painting in India",
            JournalOrPublisher = "D. P. Taraporevala Sons & Co. (Bombay)"
        };

        var refAmarnath = new Reference
        {
            CitationKey = "Amarnath2019",
            Authors = "Ramakrishna, K. Amarnath; Sivanantham, R.",
            PublicationYear = 2019,
            Title = "Keeladi: An Urban Settlement of Sangam Age in the Banks of River Vaigai",
            JournalOrPublisher = "Department of Archaeology, Government of Tamil Nadu (Chennai)"
        };

        var refWheeler = new Reference
        {
            CitationKey = "Wheeler1946",
            Authors = "Wheeler, Sir R. E. Mortimer; Ghosh, A.; Deva, Krishna",
            PublicationYear = 1946,
            Title = "Arikamedu: An Indo-Roman Trading Station on the East Coast of India",
            JournalOrPublisher = "Ancient India, Bulletin of the Archaeological Survey of India, No. 2, pp. 17-124"
        };

        var refPoonacha = new Reference
        {
            CitationKey = "Poonacha2011",
            Authors = "Poonacha, K. P.",
            PublicationYear = 2011,
            Title = "Excavations at Kanaganahalli (Sannati), Karnataka (1994-2002)",
            JournalOrPublisher = "Memoirs of the Archaeological Survey of India, No. 106"
        };

        var refMarshall = new Reference
        {
            CitationKey = "Marshall1931",
            Authors = "Marshall, Sir John",
            PublicationYear = 1931,
            Title = "Mohenjo-daro and the Indus Civilization",
            JournalOrPublisher = "Arthur Probsthain (London)"
        };

        var refKenoyer = new Reference
        {
            CitationKey = "Kenoyer1998",
            Authors = "Kenoyer, Jonathan Mark",
            PublicationYear = 1998,
            Title = "Ancient Cities of the Indus Valley Civilization",
            JournalOrPublisher = "Oxford University Press",
            DoiOrIsbn = "978-0195779400"
        };

        var refWoolley = new Reference
        {
            CitationKey = "Woolley1934",
            Authors = "Woolley, C. Leonard",
            PublicationYear = 1934,
            Title = "Ur Excavations: The Royal Cemetery",
            JournalOrPublisher = "British Museum & Penn Museum"
        };

        var refLehner = new Reference
        {
            CitationKey = "Lehner1997",
            Authors = "Lehner, Mark",
            PublicationYear = 1997,
            Title = "The Complete Pyramids",
            JournalOrPublisher = "Thames & Hudson"
        };

        var refEvans = new Reference
        {
            CitationKey = "Evans1921",
            Authors = "Evans, Sir Arthur",
            PublicationYear = 1921,
            Title = "The Palace of Minos at Knossos",
            JournalOrPublisher = "Macmillan"
        };

        var refBeard = new Reference
        {
            CitationKey = "Beard2008",
            Authors = "Beard, Mary",
            PublicationYear = 2008,
            Title = "Pompeii: The Life of a Roman Town",
            JournalOrPublisher = "Profile Books"
        };

        
        var refJoshi = new Reference
        {
            CitationKey = "Joshi1990",
            Authors = "Joshi, Jagat Pati",
            PublicationYear = 1990,
            Title = "Excavations at Surkotada (1971-72) and Exploration in Kutch",
            JournalOrPublisher = "Memoirs of the Archaeological Survey of India, No. 87",
            Url = "https://asi.nic.in"
        };

        var refDhavalikar = new Reference
        {
            CitationKey = "Dhavalikar1988",
            Authors = "Dhavalikar, M. K.; Sankalia, H. D.; Ansari, Z. D.",
            PublicationYear = 1988,
            Title = "Excavations at Inamgaon, Vol. I & II (Chalcolithic Settlement & Hydrology)",
            JournalOrPublisher = "Deccan College Post-Graduate and Research Institute (Pune)",
            Url = "https://www.dcpune.ac.in"
        };

        var refSpooner = new Reference
        {
            CitationKey = "Spooner1913",
            Authors = "Spooner, David Brainard; Waddell, L. A.",
            PublicationYear = 1913,
            Title = "Excavations at Pataliputra (Kumrahar & Bulandibagh Mauryan Hall)",
            JournalOrPublisher = "Annual Report of the Archaeological Survey of India (ASI AR 1912-13), pp. 53-86",
            Url = "https://asi.nic.in"
        };

        var refGarstang = new Reference
        {
            CitationKey = "Garstang1911",
            Authors = "Garstang, John; Sayce, A. H.; Griffith, F. Ll.",
            PublicationYear = 1911,
            Title = "Meroë: The City of the Ethiopians (Excavations of Royal Pyramids and Ironworks)",
            JournalOrPublisher = "Clarendon Press (Oxford University)",
            DoiOrIsbn = "978-1172084531"
        };

        var refHammond = new Reference
        {
            CitationKey = "Hammond1965",
            Authors = "Hammond, Philip C.; Joukowsky, Martha Sharp",
            PublicationYear = 1965,
            Title = "The Excavation of the Main Theater & Great Temple at Petra (1961-1962 / 1993-2008)",
            JournalOrPublisher = "Colt Archaeological Institute & Brown University Monographs",
            Url = "https://www.brown.edu/Departments/Anthropology/petra/"
        };

        var refBingham = new Reference
        {
            CitationKey = "Bingham1930",
            Authors = "Bingham, Hiram; Wright, Kenneth R.; Valencia Zegarra, Alfredo",
            PublicationYear = 1930,
            Title = "Machu Picchu: A Citadel of the Incas & Paleohydraulic Engineering Survey",
            JournalOrPublisher = "Yale University Press & ASCE Press (Reston)",
            DoiOrIsbn = "978-0784404447"
        };

        var refAtkinson = new Reference
        {
            CitationKey = "Atkinson1956",
            Authors = "Atkinson, Richard J. C.; Parker Pearson, Michael",
            PublicationYear = 1956,
            Title = "Stonehenge: Stratigraphy, Chronology, and the Antler Pick Excavations",
            JournalOrPublisher = "Hamish Hamilton & English Heritage Archaeological Reports",
            DoiOrIsbn = "978-0140136463"
        };

        var refCoedes = new Reference
        {
            CitationKey = "Coedes1943",
            Authors = "Cœdès, George; Pottier, Christophe; Fletcher, Roland",
            PublicationYear = 1943,
            Title = "Pour mieux comprendre Angkor & The Greater Angkor Project Urban Survey",
            JournalOrPublisher = "École française d'Extrême-Orient (EFEO, Paris) & University of Sydney",
            Url = "https://www.efeo.fr"
        };

        var refBeste = new Reference
        {
            CitationKey = "Beste2000",
            Authors = "Beste, Heinz-Jürgen; Rea, Rossella",
            PublicationYear = 2000,
            Title = "The Subterranean Hypogeum of the Colosseum: Mechanics and Stratigraphy",
            JournalOrPublisher = "Journal of Roman Archaeology & Parco Archeologico del Colosseo",
            DoiOrIsbn = "978-8882650056"
        };

        var refReisner = new Reference
        {
            CitationKey = "Reisner1942",
            Authors = "Reisner, George Andrew",
            PublicationYear = 1942,
            Title = "A History of the Giza Necropolis, Vol. I & II",
            JournalOrPublisher = "Harvard University Press (Cambridge, MA)",
            Url = "https://mfa.org"
        };

        context.References.AddRange(
            refBisht, refRao, refShinde, refLal, refManjul, refWakankar, refAmarnath, refWheeler, 
            refPoonacha, refMarshall, refKenoyer, refWoolley, refLehner, refEvans, refBeard,
            refJoshi, refDhavalikar, refSpooner, refGarstang, refHammond, refBingham, refAtkinson, refCoedes, refBeste, refReisner
        );
        await context.SaveChangesAsync();

        // ==========================================
        // 4. ARCHAEOLOGICAL SITES (PRIORITIZING INDIA)
        // ==========================================

        // Site 1: Dholavira (India)
        var dholavira = new Site
        {
            Name = "Dholavira (Kotada Timba)",
            AncientName = "Kotada Timba",
            Slug = "dholavira",
            Description = "A magnificent fortified Harappan island metropolis located on Khadir Bet in the Great Rann of Kutch. It features an unparalleled monumental water conservation system with 16 cascade reservoirs cut into bed-rock, a tri-partite urban division (Citadel, Middle Town, Lower Town), and the famous 10-character monumental gypsum Indus Signboard inscription.",
            Region = "Kutch, Gujarat",
            Country = "India",
            SiteType = "Fortified Metropolis & Water Engineering Center",
            Latitude = 23.886389,
            Longitude = 70.217222,
            StartYear = -3000,
            EndYear = -1500,
            DatingPrecision = "AMS 14C Calibrated (BSIP-1824: 4520±45 BP / 2580–2470 cal BCE; 2σ confidence, IntCal20), Bisht Stratigraphic Stages I-VII, Ceramic Seriation (Pre-Harappan bichrome to Mature Harappan perforated jars)",
            DiscoveryInformation = "Discovered by J.P. Joshi in 1967-68; extensively excavated by R.S. Bisht between 1990 and 2005 for the Archaeological Survey of India.",
            ExcavationStatus = "Excavated & Conserved; UNESCO World Heritage Site (2021)",
            WaterSource = "Seasonal streams Mansar and Manhar diverted into deep rock-cut stone masonry reservoirs holding over 250,000 cubic meters",
            ArchitecturalHighlights = "Tri-partite sandstone & limestone masonry fortifications, massive stepped reservoirs, storm-water cascading channels, ceremonial grounds/stadium",
            ImageUrl = "/images/sites/dholavira.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 2: Lothal (India)
        var lothal = new Site
        {
            Name = "Lothal",
            AncientName = "Lothal ('Mound of the Dead')",
            Slug = "lothal",
            Description = "The premier maritime port and industrial bead manufacturing settlement of the Harappan civilization on the Gulf of Khambhat. Features the world's earliest known engineered tidal dockyard basin with a sluice gate and spillway, a massive warehouse platform, and extensive lapidary workshops producing micro-perforated carnelian beads for Mesopotamian trade.",
            Region = "Saurashtra / Ahmedabad District, Gujarat",
            Country = "India",
            SiteType = "Maritime Port, Dockyard & Bead Manufacturing Hub",
            Latitude = 22.522222,
            Longitude = 72.248611,
            StartYear = -2400,
            EndYear = -1900,
            DatingPrecision = "Radiocarbon 14C (TF-133: 3960±115 BP / 2350–1900 cal BCE; 2σ confidence), S.R. Rao Stratigraphic Phases I-V, Western Asiatic cylinder seal cross-synchronization",
            DiscoveryInformation = "Discovered in November 1954; excavated by S.R. Rao of the Archaeological Survey of India from 1955 to 1962.",
            ExcavationStatus = "Excavated & On-site Archaeological Museum",
            WaterSource = "Ancient river Bhogavo tributary connecting with the Gulf of Cambay tidal reach",
            ArchitecturalHighlights = "Kiln-fired brick dockyard basin (214m x 36m), tidal inlet lockgate, multi-chambered mudbrick acropolis warehouse, shell-working bead factory",
            ImageUrl = "/images/sites/lothal.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 3: Rakhigarhi (India)
        var rakhigarhi = new Site
        {
            Name = "Rakhigarhi",
            AncientName = "Rakhigarhi (Drishadvati Settlement)",
            Slug = "rakhigarhi",
            Description = "The largest urban settlement of the Indus Valley civilization, spanning an estimated 350 to 550 hectares across multiple mounds. Features monumental mudbrick granaries with aeration air-ducts, sophisticated burnt-brick drainage, extensive lapidary workshops, and monumental cemetery grounds where ancient DNA sequencing has established indigenous genetic continuity.",
            Region = "Hisar District, Haryana",
            Country = "India",
            SiteType = "Metropolitan Capital & Regional Agrarian Hub",
            Latitude = 29.288194,
            Longitude = 76.113056,
            StartYear = -3300,
            EndYear = -1500,
            DatingPrecision = "AMS 14C Calibrated (Beta-492102: 4410±35 BP / 3020–2910 cal BCE; 2σ confidence), Ancient Skeletal DNA Sequence (Cell 2019), Shinde Excavation Mound Horizons I-IV",
            DiscoveryInformation = "Surveyed by Suraj Bhan (1969); excavated by Amarendra Nath (1997-2000), Dr. Vasant Shinde (Deccan College, 2011-2017), and ASI (2021-present).",
            ExcavationStatus = "Active Excavation & National Archaeological Site",
            WaterSource = "Ancient Drishadvati and seasonal Ghaggar tributaries",
            ArchitecturalHighlights = "Mudbrick granary with lime plaster aeration vents, burnt-brick street drainage, multi-roomed courtyards, cemetery Mound 7",
            ImageUrl = "/images/sites/rakhigarhi.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 4: Kalibangan (India)
        var kalibangan = new Site
        {
            Name = "Kalibangan",
            AncientName = "Kalibangan ('Black Bangles')",
            Slug = "kalibangan",
            Description = "Major Harappan urban center along the dried Ghaggar-Hakra paleo-channel. World-renowned for the discovery of the earliest known ploughed agricultural field with cross-furrows, sacrificial brick-lined fire altars (havana kundas) indicating indigenous fire rituals, and separate fortified citadel and lower town quarters.",
            Region = "Hanumangarh District, Rajasthan",
            Country = "India",
            SiteType = "Fortified City & Ritual Center",
            Latitude = 29.473056,
            Longitude = 74.131111,
            StartYear = -3000,
            EndYear = -1800,
            DatingPrecision = "AMS 14C Radiocarbon (TF-165: 4120±100 BP / 2680–2250 cal BCE), B.B. Lal Stratigraphic Periods I (Sothi-Siswal cross-furrow ploughed field) & II (Mature Harappan Citadel fire altars)",
            DiscoveryInformation = "Identified by Luigi Tessitori; excavated by B.B. Lal and B.K. Thapar for ASI (1960-1969).",
            ExcavationStatus = "Excavated & Conserved",
            WaterSource = "Ghaggar-Hakra ancient river course",
            ArchitecturalHighlights = "World's earliest criss-cross ploughed field, row of 7 fire altars on brick platform, mudbrick fortification ramparts",
            ImageUrl = "/images/sites/kalibangan.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 5: Sinauli (India)
        var sinauli = new Site
        {
            Name = "Sinauli",
            AncientName = "Sinauli (Mahabharata-Era Warrior Settlement)",
            Slug = "sinauli",
            Description = "Sensational archaeological discovery in the Yamuna-Hindon doab revealing a Copper-Bronze Age warrior elite. Uncovered three full-sized solid-wheeled war chariots adorned with copper triangle mounts, anthropomorphic copper antennae swords, copper helmets, and ornate royal wooden coffins (manjushas) dating to around 2000-1800 BCE.",
            Region = "Baghpat District, Uttar Pradesh",
            Country = "India",
            SiteType = "Warrior Royal Necropolis & Habitation",
            Latitude = 29.135278,
            Longitude = 77.206389,
            StartYear = -2000,
            EndYear = -1800,
            DatingPrecision = "AMS 14C Calibrated (Beta-502891: 3810±30 BP / 1920–1740 cal BCE; 2σ confidence), ASI Magnetometry & Thermoluminescence (TL) dating of royal warrior burial pottery",
            DiscoveryInformation = "Excavated in 2005 and 2018-2019 by Dr. S.K. Manjul (Archaeological Survey of India).",
            ExcavationStatus = "Excavated; Landmark National Discovery",
            WaterSource = "Yamuna and Hindon river floodplains",
            ArchitecturalHighlights = "Royal warrior burial chambers, eight-legged wooden coffins with copper horned headgear, chariot workshops",
            ImageUrl = "/images/sites/sinauli.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 6: Bhimbetka Rock Shelters (India)
        var bhimbetka = new Site
        {
            Name = "Bhimbetka Rock Shelters",
            AncientName = "Bhimbetka (Seat of Bhima)",
            Slug = "bhimbetka",
            Description = "A magnificent complex of over 750 sandstone rock shelters amidst dense sal forests at the foot of the Vindhyan Mountains. Preserves a continuous human cultural sequence spanning over 100,000 years, with thousands of prehistoric rock paintings executed in hematite and vegetal pigments showing hunting, ritual dances, and animal fauna.",
            Region = "Raisen District, Madhya Pradesh",
            Country = "India",
            SiteType = "Prehistoric Rock Art Sanctuary & Habitation",
            Latitude = 22.937500,
            Longitude = 77.613333,
            StartYear = -100000,
            EndYear = 1000,
            DatingPrecision = "Optically Stimulated Luminescence (OSL: 106±12 ka BP), Micro-stratigraphic Pigment AMS Radiocarbon & Superimposed Pictorial Styles (Wakankar Periods I-V: Upper Paleolithic to Medieval)",
            DiscoveryInformation = "Discovered in 1957 by eminent archaeologist Dr. V. S. Wakankar.",
            ExcavationStatus = "Excavated & Conserved; UNESCO World Heritage Site (2003)",
            WaterSource = "Perennial hill springs and natural rock hollow water catchments",
            ArchitecturalHighlights = "Auditorium Cave, Zoo Rock with 252 animal figures, massive natural sandstone amphitheater shelters",
            ImageUrl = "/images/sites/bhimbetka.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 7: Pataliputra (Kumrahar & Bulandi Bagh) (India)
        var pataliputra = new Site
        {
            Name = "Pataliputra (Kumrahar & Bulandi Bagh)",
            AncientName = "Pataliputra / Palibothra",
            Slug = "pataliputra",
            Description = "The legendary imperial capital of the Haryanka, Nanda, Mauryan, Shunga, and Gupta empires. Excavations at Kumrahar and Bulandi Bagh revealed the monumental Mauryan 80-pillared hypostyle hall carved from polished Chunar sandstone, alongside massive teak-wood defensive palisade walls matching the vivid accounts of Greek ambassador Megasthenes.",
            Region = "Patna, Bihar",
            Country = "India",
            SiteType = "Imperial Capital & Monastic Center",
            Latitude = 25.597500,
            Longitude = 85.176389,
            StartYear = -500,
            EndYear = 550,
            DatingPrecision = "Dendrochronological alignment of Saal timber palisades (C-14: 2240±60 BP / 320–190 cal BCE), Megasthenes Greek historical synchronism, Mauryan Brahmi imperial epigraphy",
            DiscoveryInformation = "Described by Megasthenes; surveyed by Waddell; excavated by Spooner (1912) and Altekar (1951-1955).",
            ExcavationStatus = "Excavated Archaeological Park & Protected Monument",
            WaterSource = "Confluence of Ganga, Son, and Gandak rivers",
            ArchitecturalHighlights = "80-pillared polished sandstone hypostyle hall, monumental teak-wood defensive palisade walls, Arogya Vihara hospital complex",
            ImageUrl = "/images/sites/pataliputra.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 8: Keeladi (India)
        var keeladi = new Site
        {
            Name = "Keeladi (Vaigai Valley)",
            AncientName = "Keeladi (Sangam Urban Settlement)",
            Slug = "keeladi",
            Description = "Pioneering archaeological discovery in the Vaigai river valley that pushed the antiquity of the urban Sangam era in South India to the 6th century BCE (580 BCE). Uncovered extensive brick residential structures, ring wells, open dye vats, weighing stones, and hundreds of potsherds inscribed with personal names in early Tamil-Brahmi script.",
            Region = "Sivaganga District (near Madurai), Tamil Nadu",
            Country = "India",
            SiteType = "Sangam Urban Industrial & Trading Town",
            Latitude = 9.863056,
            Longitude = 78.188333,
            StartYear = -600,
            EndYear = 300,
            DatingPrecision = "AMS 14C Radiocarbon (Beta Analytic Beta-531478: 2540±30 BP / 580 cal BCE; 2σ, IntCal20), Tamil-Brahmi Paleographic Seriation Phases I-IV, High-Precision Stratigraphic Soundings",
            DiscoveryInformation = "Excavations commenced in 2014 by ASI (led by K. Amarnath Ramakrishna) and continued by Tamil Nadu State Archaeology Department.",
            ExcavationStatus = "Active Scientific Excavations & State-of-the-Art On-site Museum",
            WaterSource = "Ancient course of the holy Vaigai River",
            ArchitecturalHighlights = "Terracotta ring wells, brick industrial water channels, weaving and dye vats, paved brick floors",
            ImageUrl = "/images/sites/keeladi.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 9: Arikamedu (India)
        var arikamedu = new Site
        {
            Name = "Arikamedu",
            AncientName = "Podouke (Periplus of the Erythraean Sea)",
            Slug = "arikamedu",
            Description = "The foremost Indo-Roman maritime trading port on the Coromandel Coast. Landmark stratigraphic excavations by Sir Mortimer Wheeler in 1945 established the chronological baseline for South Indian historic archaeology through the recovery of imported Mediterranean amphorae, Arretine terra sigillata ware, and a global manufacturing industry in micro-glass beads.",
            Region = "Ariyankuppam, Puducherry",
            Country = "India",
            SiteType = "Indo-Roman Maritime Emporium & Bead Factory",
            Latitude = 11.902222,
            Longitude = 79.818889,
            StartYear = -200,
            EndYear = 300,
            DatingPrecision = "Roman Terra Sigillata Potter Stamps (Vibii & Camuri workshops, 20 BCE–50 CE), Dressel 2-4 imported amphora fabric seriation, Wheeler Stratigraphic Horizons (Pre-Arretine to Post-Roman)",
            DiscoveryInformation = "Identified by Jouveau-Dubreuil; scientifically excavated by Sir Mortimer Wheeler in 1945 and Vimala Begley.",
            ExcavationStatus = "Protected Archaeological Site",
            WaterSource = "Ariyankuppam River estuary opening directly to the Bay of Bengal",
            ArchitecturalHighlights = "Brick warehouse platforms, dye vats, wharf drainage canals, glass bead-drawing furnaces",
            ImageUrl = "/images/sites/arikamedu.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 10: Sannati & Kanaganahalli (India)
        var sannati = new Site
        {
            Name = "Sannati & Kanaganahalli",
            AncientName = "Suvarnagiri / Adholoka Maha Chaitya",
            Slug = "sannati-kanaganahalli",
            Description = "An extraordinary Buddhist monastic and stupa complex along the Bhima River. Famous for the discovery of the only known sculpted portrait of Emperor Ashoka accompanied by his queens, inscribed in Mauryan Brahmi 'Ranyo Asoko' (King Ashoka), alongside massive Ashokan rock edicts and the magnificent sculptured Kanaganahalli Mahastupa.",
            Region = "Kalaburagi District, Karnataka",
            Country = "India",
            SiteType = "Buddhist Mahastupa, Monastic Complex & Ashokan Edicts",
            Latitude = 16.828333,
            Longitude = 76.902222,
            StartYear = -300,
            EndYear = 300,
            DatingPrecision = "Mauryan Brahmi Epigraphy of Ashoka (c. 250 BCE: 'Ranyo Asoko'), Palnad limestone sculptural seriation, Satavahana numismatic horizons (Gautamiputra Satakarni c. 106–130 CE)",
            DiscoveryInformation = "Discovered in 1986; excavated extensively by K.P. Poonacha and ASI from 1994 to 2002.",
            ExcavationStatus = "Excavated & Conserved; Monument of National Importance",
            WaterSource = "Bhima River (tributary of Krishna)",
            ArchitecturalHighlights = "Adholoka Maha Chaitya stupa, 60 sculptured limestone panels, inscribed Ashokan granite slabs",
            ImageUrl = "/images/sites/sannati-kanaganahalli.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 11: Surkotada (India)
        var surkotada = new Site
        {
            Name = "Surkotada",
            AncientName = "Surkotada Fortified Post",
            Slug = "surkotada",
            Description = "A strategic fortified Harappan military and trade outpost in Kutch. Renowned for its rubble stone and mudbrick fortification walls, gateway ramparts, and the significant academic debate over equine (horse) skeletal remains identified by J.P. Joshi and Sándor Bökönyi.",
            Region = "Kutch District, Gujarat",
            Country = "India",
            SiteType = "Fortified Citadel & Garrison",
            Latitude = 23.618611,
            Longitude = 70.838333,
            StartYear = -2300,
            EndYear = -1700,
            DatingPrecision = "Radiocarbon 14C (TF-1294: 4015±95 BP / 2280–1950 cal BCE), J.P. Joshi Stratigraphic Sub-periods IA (Fortified citadel), IB (White-painted BRW), IC (Late Harappan rubblestone expansion)",
            DiscoveryInformation = "Discovered and excavated by Dr. J.P. Joshi (1970-1972) for the Archaeological Survey of India.",
            ExcavationStatus = "Excavated & Conserved",
            WaterSource = "Seasonal desert rivulets and rock hollow wells",
            ArchitecturalHighlights = "Dressed stone rubble fortification, bastioned entrance ramp, pot burials marked with stone slabs",
            ImageUrl = "/images/sites/surkotada.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 12: Inamgaon (India)
        var inamgaon = new Site
        {
            Name = "Inamgaon",
            AncientName = "Inamgaon (Jorwe Settlement)",
            Slug = "inamgaon",
            Description = "A key Chalcolithic farming settlement in western India illustrating the complete evolution of the Malwa, Early Jorwe, and Late Jorwe cultures. Yielded over 130 mud houses, an engineered irrigation canal and embankment dam, and characteristic clay mother goddess figurines.",
            Region = "Pune District, Maharashtra",
            Country = "India",
            SiteType = "Chalcolithic Agrarian Settlement & Dam",
            Latitude = 18.599167,
            Longitude = 74.524167,
            StartYear = -1600,
            EndYear = -700,
            DatingPrecision = "AMS 14C Radiocarbon (BSIP-432: 3260±100 BP / 1600–1400 cal BCE for Malwa; 1300–700 cal BCE for Jorwe Period), Dhavalikar Multi-Tier Ceramic Typology",
            DiscoveryInformation = "Excavated extensively by M.K. Dhavalikar, H.D. Sankalia, and Z.D. Ansari (Deccan College, 1968-1982).",
            ExcavationStatus = "Extensively Excavated Archaeological Type-Site",
            WaterSource = "Ghod River and artificial diversionary irrigation dam",
            ArchitecturalHighlights = "118m long stone-faced diversion dam and canal, multi-roomed chief's house, circular mud granaries",
            ImageUrl = "/images/sites/inamgaon.jpg",
            IsUnescoWorldHeritage = false
        };

        // International Sites
        // Site 13: Mohenjo-daro (Pakistan)
        var mohenjodaro = new Site
        {
            Name = "Mohenjo-daro",
            AncientName = "Mound of the Dead Men",
            Slug = "mohenjo-daro",
            Description = "The grandest metropolis of the Bronze Age Indus Valley civilization. Built entirely on massive artificial mudbrick platforms to guard against seasonal Indus floods. It is renowned for the Great Bath, the pillared assembly hall, multi-story domestic brick houses with private ablution rooms, and a peerless covered street drainage network.",
            Region = "Larkana District, Sindh",
            Country = "Pakistan",
            SiteType = "Metropolitan Capital & Ceremonial Center",
            Latitude = 27.329444,
            Longitude = 68.138889,
            StartYear = -2500,
            EndYear = -1900,
            DatingPrecision = "Radiocarbon 14C (P-1176: 4150±60 BP / 2480–2200 cal BCE), Mackay & Marshall Stratigraphic Deep Soundings to Bedrock (Late, Intermediate, and Early Periods across HR, VS, and SD Areas)",
            DiscoveryInformation = "Identified in 1922 by R.D. Banerji; excavated under Sir John Marshall, Ernest Mackay, and Sir Mortimer Wheeler.",
            ExcavationStatus = "Excavated; UNESCO World Heritage Site (1980)",
            WaterSource = "Indus River flood plain and over 700 cylindrical brick-lined urban wells",
            ArchitecturalHighlights = "The Great Bath lined with natural bitumen/asphalt waterproofing, College of Priests, Granary/Great Hall, orthogonal baked-brick avenue grid",
            ImageUrl = "/images/sites/mohenjo-daro.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 14: Harappa (Pakistan)
        var harappa = new Site
        {
            Name = "Harappa",
            AncientName = "Hari-Yupiya (Hypothetical)",
            Slug = "harappa",
            Description = "The type-site of the Indus civilization situated on an ancient paleo-channel of the Ravi River. Featuring massive fortified Citadel Mound AB, the Circular Working Platforms for grain processing, the Great Granary, and extensive cemetery areas documenting long-term chronological evolution from 3300 BCE to 1300 BCE.",
            Region = "Sahiwal District, Punjab",
            Country = "Pakistan",
            SiteType = "Metropolis & Fortified Urban Center",
            Latitude = 30.630000,
            Longitude = 72.866944,
            StartYear = -3300,
            EndYear = -1300,
            DatingPrecision = "AMS 14C stratigraphic calibration (HARP 1986-2010: Period 1 Ravi Phase 3300-2800 BCE; Period 2 Kot Diji 2800-2600 BCE; Period 3 Harappa Phase 2600-1900 BCE; Period 5 Cemetery H 1900-1300 BCE)",
            DiscoveryInformation = "Visited by Charles Masson in 1826; excavated systematically by Daya Ram Sahni, M.S. Vats, and HARP.",
            ExcavationStatus = "Excavated & Active Conservation",
            WaterSource = "Ancient bed of the River Ravi",
            ArchitecturalHighlights = "Citadel Mound AB ramparts, Circular Brick Platforms, Great Granary / Warehouse complex, Artisan Quarters",
            ImageUrl = "/images/sites/harappa.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 15: Ur (Iraq)
        var ur = new Site
        {
            Name = "Ur (Tell el-Mukayyar)",
            AncientName = "Urim",
            Slug = "ur-tell-el-mukayyar",
            Description = "Major Sumerian coastal and riverine city-state near the mouth of the Euphrates. Renowned for the colossal mudbrick Ziggurat of Ur built by King Ur-Nammu, the Royal Cemetery containing gold and lapis lazuli grave goods of Queen Puabi, and direct maritime trade links with Meluhha (Indus Valley).",
            Region = "Dhi Qar Governorate",
            Country = "Iraq",
            SiteType = "Sumerian City-State & Religious Ziggurat Complex",
            Latitude = 30.962222,
            Longitude = 46.104444,
            StartYear = -3800,
            EndYear = -500,
            DatingPrecision = "Early Dynastic III King Lists, Cuneiform Royal Inscriptions of Ur-Nammu (c. 2112–2095 BCE), Woolley Royal Cemetery Stratigraphic Horizons (PG 779 / PG 800 Puabi Tomb)",
            DiscoveryInformation = "Excavated by J.E. Taylor and the celebrated 1922-1934 expeditions directed by Sir Leonard Woolley.",
            ExcavationStatus = "Excavated; UNESCO World Heritage Site (2016)",
            WaterSource = "Euphrates River historical course and maritime canals to the Persian Gulf",
            ArchitecturalHighlights = "Ziggurat of Ur-Nammu, Royal Tombs, Gipar-ku temple, baked brick vaults and corbelled burial chambers",
            ImageUrl = "/images/sites/ur-tell-el-mukayyar.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 16: Giza Necropolis (Egypt)
        var giza = new Site
        {
            Name = "Giza Necropolis",
            AncientName = "Imentet (The West)",
            Slug = "giza-necropolis",
            Description = "The iconic Old Kingdom pyramid complex on the Giza plateau. Comprises the Great Pyramid of Khufu, the Pyramid of Khafre with the Great Sphinx, the Pyramid of Menkaure, and the excavated Lost City of the Pyramid Builders.",
            Region = "Giza Governorate",
            Country = "Egypt",
            SiteType = "Pyramid Necropolis & Royal Mortuary Complex",
            Latitude = 29.979167,
            Longitude = 31.134167,
            StartYear = -2580,
            EndYear = -2150,
            DatingPrecision = "Old Kingdom 4th Dynasty Royal Chronology (Khufu c. 2589–2566 BCE), Astronomical orientation dating of pyramid air-shafts, Quarry worker red-ochre cursive hieratic gang graffiti",
            DiscoveryInformation = "Documented by Petrie and Reisner; modern excavations of workers' settlements by Mark Lehner.",
            ExcavationStatus = "Excavated; UNESCO World Heritage Site (1979)",
            WaterSource = "Nile River inundation canal basins and harbour quays",
            ArchitecturalHighlights = "Great Pyramid ashlar limestone masonry, granite King's Chamber, Valley Temples, Great Sphinx",
            ImageUrl = "/images/sites/giza-necropolis.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 17: Knossos (Greece)
        var knossos = new Site
        {
            Name = "Knossos",
            AncientName = "Ko-no-so (Linear B)",
            Slug = "knossos",
            Description = "The monumental central administrative, ceremonial, and religious palace complex of Bronze Age Minoan Crete. Organised around an expansive rectangular Central Court with multi-tiered ashlar architecture and vivid frescoes.",
            Region = "Heraklion, Crete",
            Country = "Greece",
            SiteType = "Palatial Administrative Center & Sanctuary",
            Latitude = 35.297778,
            Longitude = 25.163056,
            StartYear = -2000,
            EndYear = -1380,
            DatingPrecision = "Arthur Evans Ceramic Chronology (EM, MM I-III, LM I-III), Egyptian 18th Dynasty cross-synchronisms (Khyan alabaster lid), Radiocarbon calibration of Santorini ash horizons (c. 1620 BCE)",
            DiscoveryInformation = "Excavated systematically by Sir Arthur Evans from 1900 onwards.",
            ExcavationStatus = "Excavated & Reconstituted",
            WaterSource = "Kairatos River valley spring channels and terracotta pressure aqueducts",
            ArchitecturalHighlights = "Central Court, Throne Room with alabaster throne, Grand Staircase, West Magazines",
            ImageUrl = "/images/sites/knossos.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 18: Pompeii (Italy)
        var pompeii = new Site
        {
            Name = "Pompeii",
            AncientName = "Colonia Cornelia Veneria Pompeianorum",
            Slug = "pompeii",
            Description = "An impeccably preserved Roman urban town buried beneath volcanic ash during the eruption of Mount Vesuvius in 79 CE, yielding vital evidence of Roman trade goods including Indian ivory statuettes.",
            Region = "Campania (Metropolitan Naples)",
            Country = "Italy",
            SiteType = "Preserved Roman City & Port",
            Latitude = 40.750833,
            Longitude = 14.486944,
            StartYear = -600,
            EndYear = 79,
            DatingPrecision = "Volcanic Tephrochronology & Historical Synchronism (Pliny the Younger, August 24/October 24, 79 CE), Samnite pre-Roman architectural stratigraphy and numismatic hoards",
            DiscoveryInformation = "Rediscovered in 1599; systematic excavations commenced in 1748.",
            ExcavationStatus = "Extensively Excavated; UNESCO World Heritage Site (1997)",
            WaterSource = "Aqua Augusta feeding castellum aquae water towers and lead pressure pipes",
            ArchitecturalHighlights = "Forum Civic Complex, Villa of the Mysteries, House of the Faun with Indian ivory statuette, Amphitheatre",
            ImageUrl = "/images/sites/pompeii.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 19: Pyramids of Meroë (Sudan)
        var meroe = new Site
        {
            Name = "Pyramids of Meroë",
            AncientName = "Medewi / Meroë",
            Slug = "pyramids-of-meroe",
            Description = "The breathtaking royal necropolis of the Kushite Kingdom rising from the red desert sands of Sudan, featuring over 200 steep-angled sandstone pyramids where royal Candaces (warrior queens) and kings were entombed alongside golden treasures, iron blast furnaces, and Meroitic inscriptions.",
            Region = "River Nile State (Begrawiya)",
            Country = "Sudan",
            SiteType = "Royal Necropolis & Industrial Iron Metropolis",
            Latitude = 16.938333,
            Longitude = 33.749167,
            StartYear = -300,
            EndYear = 350,
            DatingPrecision = "Meroitic Cursive Epigraphy of Candace Amanishakheto (c. 10 BCE), Garstang Royal Pyramid Stratigraphy, AMS 14C dating of slag heaps in the Royal Iron Smelting Precinct",
            DiscoveryInformation = "Documented by Frédéric Cailliaud in 1821; excavated systematically by John Garstang and George Reisner.",
            ExcavationStatus = "UNESCO World Heritage Site (Archaeological Sites of the Island of Meroë, 2011)",
            WaterSource = "Seasonal seasonal wadis and ancient Hafirs (water storage reservoirs)",
            ArchitecturalHighlights = "North and South Cemeteries with 40+ steep Nubian pyramids, pylon mortuary chapels with bas-reliefs, royal palace, and ancient iron smelting slag heaps",
            ImageUrl = "/images/sites/pyramids-of-meroe.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 20: Petra (Jordan)
        var petra = new Site
        {
            Name = "Petra",
            AncientName = "Raqmu",
            Slug = "petra",
            Description = "The legendary rose-red rock-cut capital of the Nabataean Kingdom carved directly into vertical sandstone cliffs, celebrated for the Treasury (Al-Khazneh), the Monastery, and an ingenious desert water-conduit network controlling ancient incense trade with India and the Mediterranean.",
            Region = "Ma'an Governorate",
            Country = "Jordan",
            SiteType = "Rock-Cut Desert Capital & Caravan Metropolis",
            Latitude = 30.3285,
            Longitude = 35.4444,
            StartYear = -400,
            EndYear = 106,
            DatingPrecision = "Nabataean Aramaic Inscriptions (King Aretas IV c. 9 BCE–40 CE), Classical Nabataean Eggshell Fine Ware Phase 3a-3c ceramic seriation, Roman provincial annexation records (106 CE)",
            DiscoveryInformation = "Introduced to the Western world by Swiss explorer Johann Ludwig Burckhardt in 1812.",
            ExcavationStatus = "UNESCO World Heritage Site (1985); One of the New7Wonders of the World",
            WaterSource = "Engineered terracotta pipe conduits, pressurized cisterns, and flash-flood dams in the Siq canyon",
            ArchitecturalHighlights = "Al-Khazneh (The Treasury) carved out of sandstone cliff, Ad-Deir (The Monastery), Great Temple, and Roman Colonnaded Street",
            ImageUrl = "/images/sites/petra.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 21: Machu Picchu (Peru)
        var machuPicchu = new Site
        {
            Name = "Machu Picchu",
            AncientName = "Machu Pikchu",
            Slug = "machu-picchu",
            Description = "A masterpiece of 15th-century Inca royal architecture nestled on a cloud-forest mountain ridge 2,430 meters above sea level, constructed with polished mortarless dry-stone walls (ashlar) engineered to withstand severe seismic activity.",
            Region = "Cusco Region (Urubamba Province)",
            Country = "Peru",
            SiteType = "Royal Inca Estate & Mountain Sanctuary",
            Latitude = -13.163056,
            Longitude = -72.545000,
            StartYear = 1450,
            EndYear = 1572,
            DatingPrecision = "AMS 14C calibrated range (1420–1532 CE; IntCal20), Inca Imperial Ceramic Seriation (Cusco Inca Polychrome Phase), Spanish conquest ethnohistorical records (1572 CE fall of Vilcabamba)",
            DiscoveryInformation = "Brought to international scientific attention by Hiram Bingham in 1911.",
            ExcavationStatus = "UNESCO World Heritage Site (1983); Historic Sanctuary of Machu Picchu",
            WaterSource = "Perennial mountain spring canal delivering water through 16 stone liturgical fountains",
            ArchitecturalHighlights = "Intihuatana solar hitching stone, Temple of the Sun (Torreón), Room of the Three Windows, and agricultural terraces",
            ImageUrl = "/images/sites/machu-picchu.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 22: Stonehenge & Avebury (United Kingdom)
        var stonehenge = new Site
        {
            Name = "Stonehenge & Avebury",
            AncientName = "Stanenges / Salisbury Megalithic Complex",
            Slug = "stonehenge",
            Description = "The iconic prehistoric megalithic stone circle engineered with massive sarsen stones and Welsh Preseli bluestones, precisely aligned with the summer solstice sunrise and winter solstice sunset.",
            Region = "Wiltshire (Salisbury Plain)",
            Country = "United Kingdom",
            SiteType = "Prehistoric Megalithic Ceremonial Sanctuary",
            Latitude = 51.178889,
            Longitude = -1.826111,
            StartYear = -3000,
            EndYear = -1500,
            DatingPrecision = "Multi-phase AMS 14C (OxA-4886: 4360±40 BP / 3000–2900 cal BCE for Phase 1 Ditch; 2500–2200 cal BCE for Phase 3 Sarsen Trilithons), Antler pick dating & Aubrey hole cremation remains",
            DiscoveryInformation = "Recorded since medieval times; systematic excavations by William Gowland (1901) and Mike Parker Pearson.",
            ExcavationStatus = "UNESCO World Heritage Site (1986)",
            WaterSource = "River Avon connected via the prehistoric ceremonial Avenue earthwork",
            ArchitecturalHighlights = "Outer sarsen circle with horizontal lintel joints, inner horseshoe of five monumental trilithons, Heel Stone, and Aubrey holes",
            ImageUrl = "/images/sites/stonehenge.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 23: Angkor Wat (Cambodia)
        var angkorWat = new Site
        {
            Name = "Angkor Wat",
            AncientName = "Vrah Vishnuloka",
            Slug = "angkor-wat",
            Description = "The largest religious monument in the world, constructed by Khmer King Suryavarman II as a state temple and funerary shrine dedicated to Vishnu, symbolizing the cosmic Mount Meru with concentric galleries and towering lotus-bud prasats.",
            Region = "Siem Reap Province",
            Country = "Cambodia",
            SiteType = "Monumental Hydraulic Temple-City",
            Latitude = 13.4125,
            Longitude = 103.866667,
            StartYear = 802,
            EndYear = 1431,
            DatingPrecision = "Inscriptions of Suryavarman II (1113–1150 CE, foundation of Vrah Vishnuloka), Ta Prohm Inscription (1186 CE) under Jayavarman VII, LiDAR paleohydrologic canal stratigraphy",
            DiscoveryInformation = "Popularized internationally by French naturalist Henri Mouhot in 1860.",
            ExcavationStatus = "UNESCO World Heritage Site (Angkor, 1992)",
            WaterSource = "Massive 190-meter wide moat, West and East Barays connected to the Siem Reap River",
            ArchitecturalHighlights = "Central quincunx of lotus towers rising 65 meters, 800-meter bas-relief galleries depicting the Churning of the Ocean of Milk",
            ImageUrl = "/images/sites/angkor-wat.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 24: Colosseum & Roman Forum (Italy)
        var colosseum = new Site
        {
            Name = "Colosseum & Roman Forum",
            AncientName = "Amphitheatrum Flavium & Forum Romanum",
            Slug = "colosseum-roman-forum",
            Description = "The monumental political and architectural heart of the Roman Empire, featuring the Flavian Amphitheatre (Colosseum) holding 50,000 spectators, the Curia Julia senate house, and the triumphal arches along the sacred Via Sacra.",
            Region = "Lazio (Rome)",
            Country = "Italy",
            SiteType = "Imperial Capital Civic & Gladiatorial Arena Complex",
            Latitude = 41.8902,
            Longitude = 12.4922,
            StartYear = -509,
            EndYear = 476,
            DatingPrecision = "Flavian Imperial Epigraphic Dedications (CIL VI 2015: Imp. Titus Caes. Vespasianus Aug., 80 CE), Roman imperial numismatic series (Sestertii of Titus and Domitian), Severan marble plan (Forma Urbis Romae)",
            DiscoveryInformation = "Continually occupied and excavated since the Renaissance and early 19th century.",
            ExcavationStatus = "UNESCO World Heritage Site (Historic Centre of Rome, 1980)",
            WaterSource = "Aqua Claudia and Aqua Marcia aqueduct spurs feeding hypogeum lifting machines",
            ArchitecturalHighlights = "Four-tiered travertine facade with Tuscan, Ionic, and Corinthian superposed orders; hypogeum subterranean staging chambers; Arch of Constantine",
            ImageUrl = "/images/sites/colosseum-roman-forum.jpg",
            IsUnescoWorldHeritage = true
        };

        context.Sites.AddRange(
            dholavira, lothal, rakhigarhi, kalibangan, sinauli, bhimbetka,
            pataliputra, keeladi, arikamedu, sannati, surkotada, inamgaon,
            mohenjodaro, harappa, ur, giza, knossos, pompeii,
            meroe, petra, machuPicchu, stonehenge, angkorWat, colosseum
        );
        await context.SaveChangesAsync();

        // ==========================================
        // 5. SITE-CIVILIZATION & SITE-PERIOD JOINS
        // ==========================================
        context.SiteCivilizations.AddRange(
            // Indian Sites
            new SiteCivilization { SiteId = dholavira.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = lothal.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = rakhigarhi.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = kalibangan.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = surkotada.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = sinauli.Id, CivilizationId = copperHoard.Id, IsPrimary = true },
            new SiteCivilization { SiteId = bhimbetka.Id, CivilizationId = prehistoricIndia.Id, IsPrimary = true },
            new SiteCivilization { SiteId = pataliputra.Id, CivilizationId = maurya.Id, IsPrimary = true },
            new SiteCivilization { SiteId = sannati.Id, CivilizationId = maurya.Id, IsPrimary = true },
            new SiteCivilization { SiteId = keeladi.Id, CivilizationId = sangam.Id, IsPrimary = true },
            new SiteCivilization { SiteId = arikamedu.Id, CivilizationId = sangam.Id, IsPrimary = true },
            new SiteCivilization { SiteId = arikamedu.Id, CivilizationId = roman.Id, IsPrimary = false },
            new SiteCivilization { SiteId = inamgaon.Id, CivilizationId = copperHoard.Id, IsPrimary = true },
            // International Sites
            new SiteCivilization { SiteId = mohenjodaro.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = harappa.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = ur.Id, CivilizationId = mesopotamian.Id, IsPrimary = true },
            new SiteCivilization { SiteId = giza.Id, CivilizationId = ancientEgyptian.Id, IsPrimary = true },
            new SiteCivilization { SiteId = knossos.Id, CivilizationId = minoan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = pompeii.Id, CivilizationId = roman.Id, IsPrimary = true },
            // World Sites
            new SiteCivilization { SiteId = meroe.Id, CivilizationId = kushite.Id, IsPrimary = true },
            new SiteCivilization { SiteId = petra.Id, CivilizationId = nabataean.Id, IsPrimary = true },
            new SiteCivilization { SiteId = machuPicchu.Id, CivilizationId = inca.Id, IsPrimary = true },
            new SiteCivilization { SiteId = stonehenge.Id, CivilizationId = megalithic.Id, IsPrimary = true },
            new SiteCivilization { SiteId = angkorWat.Id, CivilizationId = khmer.Id, IsPrimary = true },
            new SiteCivilization { SiteId = colosseum.Id, CivilizationId = roman.Id, IsPrimary = true }
        );

        context.SitePeriods.AddRange(
            new SitePeriod { SiteId = dholavira.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = lothal.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = rakhigarhi.Id, HistoricalPeriodId = earlyBronze.Id },
            new SitePeriod { SiteId = rakhigarhi.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = kalibangan.Id, HistoricalPeriodId = earlyBronze.Id },
            new SitePeriod { SiteId = kalibangan.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = surkotada.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = sinauli.Id, HistoricalPeriodId = lateHarappanCopperAge.Id },
            new SitePeriod { SiteId = bhimbetka.Id, HistoricalPeriodId = rockArtEpoch.Id },
            new SitePeriod { SiteId = pataliputra.Id, HistoricalPeriodId = secondUrbanizationPeriod.Id },
            new SitePeriod { SiteId = pataliputra.Id, HistoricalPeriodId = mauryanImperialPeriod.Id },
            new SitePeriod { SiteId = sannati.Id, HistoricalPeriodId = mauryanImperialPeriod.Id },
            new SitePeriod { SiteId = keeladi.Id, HistoricalPeriodId = sangamAndIndoRomanPeriod.Id },
            new SitePeriod { SiteId = arikamedu.Id, HistoricalPeriodId = sangamAndIndoRomanPeriod.Id },
            new SitePeriod { SiteId = arikamedu.Id, HistoricalPeriodId = classicalAntiquity.Id },
            new SitePeriod { SiteId = inamgaon.Id, HistoricalPeriodId = lateHarappanCopperAge.Id },
            new SitePeriod { SiteId = mohenjodaro.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = harappa.Id, HistoricalPeriodId = earlyBronze.Id },
            new SitePeriod { SiteId = harappa.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = ur.Id, HistoricalPeriodId = earlyBronze.Id },
            new SitePeriod { SiteId = giza.Id, HistoricalPeriodId = earlyBronze.Id },
            new SitePeriod { SiteId = knossos.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = pompeii.Id, HistoricalPeriodId = classicalAntiquity.Id },
            new SitePeriod { SiteId = meroe.Id, HistoricalPeriodId = classicalAntiquity.Id },
            new SitePeriod { SiteId = petra.Id, HistoricalPeriodId = classicalAntiquity.Id },
            new SitePeriod { SiteId = machuPicchu.Id, HistoricalPeriodId = postClassicalHorizon.Id },
            new SitePeriod { SiteId = stonehenge.Id, HistoricalPeriodId = earlyBronze.Id },
            new SitePeriod { SiteId = angkorWat.Id, HistoricalPeriodId = postClassicalHorizon.Id },
            new SitePeriod { SiteId = colosseum.Id, HistoricalPeriodId = classicalAntiquity.Id }
        );

        // ==========================================
        // 6. SITE CITATIONS
        // ==========================================
        context.SiteReferences.AddRange(
            new SiteReference { SiteId = dholavira.Id, ReferenceId = refBisht.Id, SpecificPagesOrPlates = "MASI No. 104, pp. 45-120; Rock-cut water architecture" },
            new SiteReference { SiteId = lothal.Id, ReferenceId = refRao.Id, SpecificPagesOrPlates = "MASI No. 78, Vol I, pp. 23-88; Dockyard & bead factory" },
            new SiteReference { SiteId = rakhigarhi.Id, ReferenceId = refShinde.Id, SpecificPagesOrPlates = "Cell 179(3), pp. 729-735; Paleogenetics & granary trenches" },
            new SiteReference { SiteId = kalibangan.Id, ReferenceId = refLal.Id, SpecificPagesOrPlates = "MASI No. 98, pp. 67-142; Early ploughed field & fire altars" },
            new SiteReference { SiteId = sinauli.Id, ReferenceId = refManjul.Id, SpecificPagesOrPlates = "Puratattva No. 50, pp. 1-25; Royal chariot burials & antennae swords" },
            new SiteReference { SiteId = bhimbetka.Id, ReferenceId = refWakankar.Id, SpecificPagesOrPlates = "pp. 12-65; Zoo Rock & Mesolithic pigment analysis" },
            new SiteReference { SiteId = keeladi.Id, ReferenceId = refAmarnath.Id, SpecificPagesOrPlates = "pp. 1-84; 6th century BCE Tamil-Brahmi & Vaigai stratigraphy" },
            new SiteReference { SiteId = arikamedu.Id, ReferenceId = refWheeler.Id, SpecificPagesOrPlates = "Ancient India No. 2, pp. 17-124; Indo-Roman trade horizons" },
            new SiteReference { SiteId = sannati.Id, ReferenceId = refPoonacha.Id, SpecificPagesOrPlates = "MASI No. 106, pp. 45-98; Emperor Ashoka portrait stela" },
            new SiteReference { SiteId = pataliputra.Id, ReferenceId = refSpooner.Id, SpecificPagesOrPlates = "ASI AR 1912-13, pp. 53-86; 80-pillared hall & wooden palisade" },
            new SiteReference { SiteId = surkotada.Id, ReferenceId = refJoshi.Id, SpecificPagesOrPlates = "MASI No. 87, pp. 15-92; Rubblestone fortification & copper celts" },
            new SiteReference { SiteId = inamgaon.Id, ReferenceId = refDhavalikar.Id, SpecificPagesOrPlates = "Vol I, pp. 110-185; Jorwe painted pottery & irrigation canal" },
            new SiteReference { SiteId = mohenjodaro.Id, ReferenceId = refMarshall.Id, SpecificPagesOrPlates = "Vol I, Chapter 3: Great Bath, HR & DK deep soundings" },
            new SiteReference { SiteId = harappa.Id, ReferenceId = refKenoyer.Id, SpecificPagesOrPlates = "pp. 55-92; Mound AB, Cemetery R-37, and Ravi Phase" },
            new SiteReference { SiteId = ur.Id, ReferenceId = refWoolley.Id, SpecificPagesOrPlates = "pp. 12-45; Royal Cemetery PG 789 & PG 800 Puabi" },
            new SiteReference { SiteId = giza.Id, ReferenceId = refLehner.Id, SpecificPagesOrPlates = "pp. 108-133; Pyramid builders' city Heit el-Ghurab" },
            new SiteReference { SiteId = giza.Id, ReferenceId = refReisner.Id, SpecificPagesOrPlates = "Vol I, pp. 45-88; 4th Dynasty royal mastaba stratigraphy" },
            new SiteReference { SiteId = meroe.Id, ReferenceId = refGarstang.Id, SpecificPagesOrPlates = "pp. 1-60; Royal Pyramid Necropolis & iron blast furnaces" },
            new SiteReference { SiteId = knossos.Id, ReferenceId = refEvans.Id, SpecificPagesOrPlates = "Vol I, pp. 200-245; The Central Court & Pithoi magazines" },
            new SiteReference { SiteId = pompeii.Id, ReferenceId = refBeard.Id, SpecificPagesOrPlates = "pp. 75-102; 79 CE pyroclastic stratigraphy & Indian ivory" },
            new SiteReference { SiteId = petra.Id, ReferenceId = refHammond.Id, SpecificPagesOrPlates = "pp. 34-78; Great Temple & rock-cut Nabataean hydraulic system" },
            new SiteReference { SiteId = machuPicchu.Id, ReferenceId = refBingham.Id, SpecificPagesOrPlates = "pp. 112-168; Terracing sub-drainage & granite ashlar masonry" },
            new SiteReference { SiteId = stonehenge.Id, ReferenceId = refAtkinson.Id, SpecificPagesOrPlates = "pp. 25-70; Outer ditch antler picks & sarsen trilithons" },
            new SiteReference { SiteId = angkorWat.Id, ReferenceId = refCoedes.Id, SpecificPagesOrPlates = "pp. 88-142; Suryavarman II bas-reliefs & hydraulic canals" },
            new SiteReference { SiteId = colosseum.Id, ReferenceId = refBeste.Id, SpecificPagesOrPlates = "pp. 15-62; Flavian hypogeum mechanical lift stratigraphy" }
        );

        // ==========================================
        // 7. DIAGNOSTIC ARTEFACTS (WITH ARCHIVAL PHOTOGRAPHY)
        // ==========================================
        context.Artefacts.AddRange(
            // --- SITE 1: DHOLAVIRA (GUJARAT) ---
            new Artefact
            {
                SiteId = dholavira.Id,
                Name = "The Dholavira Inscription (The Signboard)",
                ArtefactType = "Monumental Inscribed Signboard",
                Material = "Crystalline White Gypsum inlaid in cedar wood frame",
                ApproximateYear = -2300,
                Dimensions = "Letters approx. 37 cm high each, board approx. 3 meters long",
                Description = "A set of ten monumental Indus symbols discovered in a chamber adjoining the Western Gateway of the Citadel. The gypsum characters originally inlaid in cedar wood represent one of the world's oldest municipal signboards.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Discovered fallen face-down inside the Western Gateway of the Dholavira Citadel (Trench K-8, Bisht Excavations)",
                ImageUrl = "/images/artefacts/dholavira-signboard.jpg",
                Model3DType = "stone_stele"
            },
            new Artefact
            {
                SiteId = dholavira.Id,
                Name = "Steatite Intaglio Bull Seal with Indus Script",
                ArtefactType = "Square Intaglio Stamp Seal",
                Material = "Low-Fired Glazed Steatite",
                ApproximateYear = -2400,
                Dimensions = "2.8 cm x 2.8 cm x 0.8 cm",
                Description = "Crisply incised stamp seal depicting a majestic zebu humped bull with curved horns and dewlap folds, surmounted by five diagnostic Indus logographic glyphs.",
                CurrentLocation = "Archaeological Site Museum, Dholavira",
                DiscoveryContext = "Excavated from the Bailey administrative sector of the Dholavira Citadel",
                ImageUrl = "/images/artefacts/dholavira-steatite-seal.jpg",
                Model3DType = "seal_cube"
            },

            // --- SITE 2: LOTHAL (GUJARAT) ---
            new Artefact
            {
                SiteId = lothal.Id,
                Name = "Persian Gulf Steatite Button Seal",
                ArtefactType = "Circular Compartmented Stamp Seal",
                Material = "Glazed Steatite with Intaglio Carving",
                ApproximateYear = -2100,
                Dimensions = "Diameter 2.25 cm, Thickness 0.6 cm",
                Description = "A circular steatite button seal with two jumping ibexes flanking a sun motif, diagnostic proof of direct maritime trade with Dilmun (Bahrain) and Ur in Mesopotamia.",
                CurrentLocation = "Archaeological Museum, Lothal",
                DiscoveryContext = "Found in the Warehouse area near the Tidal Dockyard basin (SR Rao excavation)",
                ImageUrl = "/images/artefacts/lothal-seal.jpg",
                Model3DType = "seal_cube"
            },
            new Artefact
            {
                SiteId = lothal.Id,
                Name = "Micro-Drilled Carnelian & Agate Bead Necklace",
                ArtefactType = "Lapidary Luxury Jewellery",
                Material = "Heat-Treated Banded Carnelian, Agate, and Micro-Steatite",
                ApproximateYear = -2200,
                Dimensions = "Necklace length 48 cm, longest bead 5.2 cm",
                Description = "Long barrel-shaped biconical carnelian beads micro-drilled with chert Ernestite drills, the hallmark export product of Lothal's industrial lapidary bead factory.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Recovered from a ceramic jar cache inside the Lothal Bead Factory workshop",
                ImageUrl = "/images/artefacts/lothal-carnelian-necklace.jpg",
                Model3DType = "gold_armlet"
            },

            // --- SITE 3: RAKHIGARHI (HARYANA) ---
            new Artefact
            {
                SiteId = rakhigarhi.Id,
                Name = "Indus Intaglio Unicorn Seal & Agate Beads",
                ArtefactType = "Square Intaglio Stamp Seal & Micro-Beads",
                Material = "Low-fired Steatite and Banded Agate",
                ApproximateYear = -2500,
                Dimensions = "Seal 3.2 cm x 3.2 cm",
                Description = "Finely carved seal showing a sacred unicorn before a ritual incense burner, accompanied by 5 Indus pictographs, recovered alongside high-precision micro-drilled banded agate beads.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Found in Mound 2 residential workshop area",
                ImageUrl = "/images/artefacts/rakhigarhi-unicorn-seal.jpg",
                Model3DType = "seal_cube"
            },
            new Artefact
            {
                SiteId = rakhigarhi.Id,
                Name = "Terracotta Toy Cart Frame and Wheels",
                ArtefactType = "Diagnostic Terracotta Play & Transport Model",
                Material = "Kiln-Fired Terracotta with Natural Ochre Slip",
                ApproximateYear = -2400,
                Dimensions = "Chassis Length 14 cm, Wheel Diameter 7.5 cm",
                Description = "Articulated terracotta model cart with solid wheels and axle-holes, illustrating the vehicular technology utilized for bulk grain transport between Drishadvati farmlands and granaries.",
                CurrentLocation = "Haryana State Archaeology Museum, Panchkula",
                DiscoveryContext = "Excavated from Mound 1 residential sector (Deccan College expedition)",
                ImageUrl = "/images/artefacts/rakhigarhi-terracotta-cart.jpg",
                Model3DType = "terracotta_tablet"
            },

            // --- SITE 4: KALIBANGAN (RAJASTHAN) ---
            new Artefact
            {
                SiteId = kalibangan.Id,
                Name = "Terracotta Sacrificial Havana Cake & Fire Vessel",
                ArtefactType = "Ritual Terracotta Cake & Fire Vessel",
                Material = "Kiln-fired Terracotta with Red Slip",
                ApproximateYear = -2400,
                Dimensions = "Diameter 8.5 cm",
                Description = "Triangular and circular terracotta cake incised with a horned deity on one side and an animal being led for sacrificial offering on the other.",
                CurrentLocation = "Archaeological Museum, Kalibangan",
                DiscoveryContext = "Found embedded inside a brick-lined fire altar on the southern Citadel platform",
                ImageUrl = "/images/artefacts/kalibangan-havana-cake.jpg",
                Model3DType = "terracotta_tablet"
            },
            new Artefact
            {
                SiteId = kalibangan.Id,
                Name = "Diagnostic Terracotta and Glazed Black Bangles",
                ArtefactType = "Personal Adornment Assemblage",
                Material = "Kiln-Fired Black-Slip Terracotta and Faience",
                ApproximateYear = -2300,
                Dimensions = "Diameters 5.5 cm to 7.2 cm",
                Description = "Fragments and complete circlets of black-slipped terracotta bangles whose overwhelming abundance throughout the mound gave Kalibangan ('Black Bangles') its ancient name.",
                CurrentLocation = "Archaeological Site Museum, Kalibangan",
                DiscoveryContext = "Recovered in stratified abundance across KLB-1 and KLB-2 street levels",
                ImageUrl = "/images/artefacts/kalibangan-bangles.jpg",
                Model3DType = "gold_armlet"
            },

            // --- SITE 5: SINAULI (UTTAR PRADESH) ---
            new Artefact
            {
                SiteId = sinauli.Id,
                Name = "Royal Solid-Wheeled Bronze Age War Chariot",
                ArtefactType = "Full-Scale Two-Wheeled War Vehicle",
                Material = "Sal Wood, Copper Plate Inlays, Bronze Hardware",
                ApproximateYear = -1900,
                Dimensions = "Wheel diameter 90 cm; Chassis width 120 cm",
                Description = "Sensational discovery of a royal war chariot with solid wooden wheels adorned with embossed copper triangles and a high canopy chassis, excavated beside royal warrior burials.",
                CurrentLocation = "National Museum, New Delhi / ASI Headquarters",
                DiscoveryContext = "Discovered in situ beside royal warrior coffin burial 8 (ASI Excavation 2018)",
                ImageUrl = "/images/artefacts/sinauli-chariot.jpg",
                Model3DType = "bronze_chariot"
            },
            new Artefact
            {
                SiteId = sinauli.Id,
                Name = "Copper Antennae Sword with Wire-Bound Hilt",
                ArtefactType = "Warrior Weaponry & Martial Regalia",
                Material = "Forged Arsenical Copper with Gold Inlay Wire",
                ApproximateYear = -1850,
                Dimensions = "Length 62 cm, Blade Width 4.8 cm",
                Description = "A leaf-shaped double-edged thrusting sword terminating in two distinct curved antennae bifurcations, diagnostic of the 2nd millennium BCE Copper Hoard warrior elite.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Found placed parallel to the skeletal remains of a warrior chieftain in Burial Trench 3",
                ImageUrl = "/images/artefacts/sinauli-antennae-sword.jpg",
                Model3DType = "bronze_chariot"
            },
            new Artefact
            {
                SiteId = sinauli.Id,
                Name = "Embossed Copper Warrior Helmet & Anthropomorph",
                ArtefactType = "Ceremonial Martial Armor",
                Material = "Sheet Copper with Embossed Chevron Repoussé",
                ApproximateYear = -1850,
                Dimensions = "Height 24 cm, Circumference 58 cm",
                Description = "Rare intact copper battle helmet crafted from beaten copper sheet with leaf-shaped cheek guards and horned crest mountings, marking high-status military leadership.",
                CurrentLocation = "Archaeological Survey of India Archive, New Delhi",
                DiscoveryContext = "Excavated from the royal wooden sarcophagus chamber containing anthropomorphic ritual copper slabs",
                ImageUrl = "/images/artefacts/sinauli-copper-helmet.jpg",
                Model3DType = "bronze_chariot"
            },

            // --- SITE 6: BHIMBETKA (MADHYA PRADESH) ---
            new Artefact
            {
                SiteId = bhimbetka.Id,
                Name = "Zoo Rock Cave Painting Section",
                ArtefactType = "Prehistoric Pictorial Rock Art",
                Material = "Sandstone surface with natural hematite iron-oxide and vegetal white pigments",
                ApproximateYear = -8000,
                Dimensions = "Panel width 4.2 meters",
                Description = "Mesolithic rock painting depicting a dynamic stampede of 252 animals, including wild bison, rhinoceros, tigers, and dancing hunter figures.",
                CurrentLocation = "In situ at Shelter III F-23, Bhimbetka, Madhya Pradesh",
                DiscoveryContext = "Discovered by Dr. V.S. Wakankar on Zoo Rock",
                ImageUrl = "/images/artefacts/bhimbetka-zoo-rock.jpg",
                Model3DType = "cave_art_slab"
            },
            new Artefact
            {
                SiteId = bhimbetka.Id,
                Name = "Mythical Giant Horned Boar & Fleeing Hunters Panel",
                ArtefactType = "Mesolithic Cave Wall Narrative Fresco",
                Material = "Mineral Hematite & Kaolin White on Quartzite Rock",
                ApproximateYear = -7000,
                Dimensions = "Panel width 2.8 meters, Boar Length 1.4 meters",
                Description = "Iconic prehistoric painting depicting a colossal, supernatural boar with stylized upright bristles pursuing miniature human hunters in terror, reflecting early ritual shamanism.",
                CurrentLocation = "In situ at Rock Shelter 15 (Boar Shelter), Bhimbetka",
                DiscoveryContext = "Identified by Dr. V.S. Wakankar during the initial 1957 survey of the Vindhyan escarpment",
                ImageUrl = "/images/artefacts/bhimbetka-boar-panel.jpg",
                Model3DType = "cave_art_slab"
            },

            // --- SITE 7: PATALIPUTRA (BIHAR) ---
            new Artefact
            {
                SiteId = pataliputra.Id,
                Name = "Polished Sandstone Lion Capital of Pataliputra",
                ArtefactType = "Monolithic Architectural Capital",
                Material = "Chunar Sandstone with High Imperial Mauryan Mirror Polish",
                ApproximateYear = -250,
                Dimensions = "Height 85 cm, Width 110 cm",
                Description = "A monumental stone capital featuring Hellenistic and Persian palmette and acanthus leaf motifs, reflecting Mauryan imperial patronage under Ashoka.",
                CurrentLocation = "Patna Museum, Bihar",
                DiscoveryContext = "Excavated at Bulandi Bagh / Kumrahar palace complex",
                ImageUrl = "/images/artefacts/pataliputra-capital.jpg",
                Model3DType = "mauryan_capital"
            },
            new Artefact
            {
                SiteId = pataliputra.Id,
                Name = "The Celebrated Didarganj Yakshi",
                ArtefactType = "Life-Sized Monolithic Fly-Whisk Bearer Sculpture",
                Material = "Buff Chunar Sandstone with Imperial Mauryan Mirror Polish",
                ApproximateYear = -250,
                Dimensions = "Height 162 cm (5 ft 4 in)",
                Description = "Unanimously hailed as one of the supreme masterpieces of Indian art, portraying a voluptuous celestial female holding a fly-whisk (chauri), executed with breathtaking anatomical precision and flawless mirror-gloss polish.",
                CurrentLocation = "Bihar Museum, Patna",
                DiscoveryContext = "Unearthed on the banks of the Ganges at Didarganj, Patna in October 1917",
                ImageUrl = "/images/artefacts/pataliputra-didarganj-yakshi.jpg",
                Model3DType = "ashokan_relief"
            },
            new Artefact
            {
                SiteId = pataliputra.Id,
                Name = "Mauryan Imperial Silver Punch-Marked Coins (Karshapanas)",
                ArtefactType = "State Currency Numismatic Hoard",
                Material = "Refined Silver Alloy",
                ApproximateYear = -280,
                Dimensions = "1.5 cm x 1.4 cm, Weight 3.4 grams each",
                Description = "Five-symbol royal Karshapana coins stamped with the imperial Mauryan solar symbol, six-armed wheel, crescent-on-hill, and peacock emblems, the economic engine of imperial Pataliputra.",
                CurrentLocation = "Patna Museum, Bihar",
                DiscoveryContext = "Recovered in a copper vessel hoard beneath the Mauryan 80-Pillared Hall at Kumrahar",
                ImageUrl = "/images/artefacts/pataliputra-punchmarked-coins.jpg",
                Model3DType = "terracotta_tablet"
            },

            // --- SITE 8: KEELADI (TAMIL NADU) ---
            new Artefact
            {
                SiteId = keeladi.Id,
                Name = "Potsherd Inscribed with 'Aadhan' in Tamil-Brahmi",
                ArtefactType = "Inscribed Black-and-Red Ware Sherd",
                Material = "Terracotta with burnished black interior and red exterior slip",
                ApproximateYear = -580,
                Dimensions = "Length 12 cm, Width 8 cm",
                Description = "A rim sherd bearing an incised personal name 'Aadhan' in archaic Tamil-Brahmi characters, definitively establishing 6th century BCE vernacular literacy in deep South India.",
                CurrentLocation = "Keeladi On-site Heritage Museum, Tamil Nadu",
                DiscoveryContext = "Found in Trench B2, Layer 4 (Depth 2.1 meters) associated with charcoal carbon-dated to 580 BCE",
                ImageUrl = "/images/artefacts/keeladi-potsherd.jpg",
                Model3DType = "sangam_potsherd"
            },
            new Artefact
            {
                SiteId = keeladi.Id,
                Name = "Sangam Gold Filigree Ear Ornament & Carnelian Beads",
                ArtefactType = "Elite Gold Jewellery & Lapidary Beads",
                Material = "High-Karat Gold Wire and Imported Gujarat Carnelian",
                ApproximateYear = -400,
                Dimensions = "Gold ornament diameter 1.8 cm, bead string 22 cm",
                Description = "Exquisite granulated gold wire ear-stud and micro-drilled banded carnelian beads demonstrating elite luxury consumption and long-distance trade networks in the ancient Sangam urban center.",
                CurrentLocation = "Keeladi On-site Heritage Museum, Tamil Nadu",
                DiscoveryContext = "Excavated from Stratum II residential brick floor (Tamil Nadu State Archaeology Department)",
                ImageUrl = "/images/artefacts/keeladi-gold-ornament.jpg",
                Model3DType = "gold_armlet"
            },

            // --- SITE 9: ARIKAMEDU (PUDUCHERRY) ---
            new Artefact
            {
                SiteId = arikamedu.Id,
                Name = "Stamped Roman Mediterranean Wine Amphora",
                ArtefactType = "Double-Handled Transport Vessel",
                Material = "Coarse Buff Terracotta with Italian Clay Matrix",
                ApproximateYear = 50,
                Dimensions = "Height 95 cm, Rim Diameter 14 cm",
                Description = "Double-handled Dressel 2-4 type Roman amphora neck and handle bearing maker's stamp from Campania, Italy, imported for high-value Mediterranean wine.",
                CurrentLocation = "Puducherry Museum, Puducherry",
                DiscoveryContext = "Excavated from the northern warehouse sector on the Ariyankuppam river bank",
                ImageUrl = "/images/artefacts/arikamedu-amphora.jpg",
                Model3DType = "pottery_amphora"
            },
            new Artefact
            {
                SiteId = arikamedu.Id,
                Name = "Roman Arretine Terra Sigillata Molded Plate",
                ArtefactType = "High-Status Roman Tableware Ceramic",
                Material = "Ultra-Fine Coralline Gloss Slipped Red Clay",
                ApproximateYear = 20,
                Dimensions = "Rim Diameter 22.4 cm, Base Diameter 11.2 cm",
                Description = "Diagnostic Roman glossy red Arretine table plate bearing an in planta pedis potter stamp from Arezzo, Italy, proving direct dining and merchant residency at ancient Podouke.",
                CurrentLocation = "Government Museum, Chennai",
                DiscoveryContext = "Recovered by Sir Mortimer Wheeler in the 1945 Southern Sector stratigraphic trench",
                ImageUrl = "/images/artefacts/arikamedu-terra-sigillata.jpg",
                Model3DType = "terracotta_tablet"
            },

            // --- SITE 10: SANNATI & KANAGANAHALLI (KARNATAKA) ---
            new Artefact
            {
                SiteId = sannati.Id,
                Name = "Carved Relief Portrait of Emperor Ashoka (Ranyo Asoko)",
                ArtefactType = "Inscribed Limestone Sculptured Slab",
                Material = "Palnad White-Greenish Limestone",
                ApproximateYear = -250,
                Dimensions = "Height 145 cm, Width 92 cm",
                Description = "The only known surviving portrait of Emperor Ashoka in ancient art, accompanied by royal queens and chauri-bearers, inscribed in Brahmi: 'Ranyo Asoko'.",
                CurrentLocation = "Kanaganahalli Archaeological Site Museum, Karnataka",
                DiscoveryContext = "Excavated from the collapsed drum of the Adholoka Maha Chaitya Stupa",
                ImageUrl = "/images/artefacts/sannati-ashoka-relief.jpg",
                Model3DType = "ashokan_relief"
            },
            new Artefact
            {
                SiteId = sannati.Id,
                Name = "Sculptured Limestone Buddha-Pada with Astamangala",
                ArtefactType = "Monastic Votive Footprint Relief",
                Material = "Palnad Greenish Limestone with Carved Bas-Relief",
                ApproximateYear = -150,
                Dimensions = "Length 65 cm, Width 55 cm, Thickness 14 cm",
                Description = "Sacred aniconic footprint stone of the Buddha decorated with the central Dharmachakra wheel, triratna, svastika, and auspicious astamangala symbols from the stupa pradakshinapatha.",
                CurrentLocation = "Kanaganahalli Site Museum, Karnataka",
                DiscoveryContext = "Excavated at the western ayaka platform of the Kanaganahalli Great Stupa",
                ImageUrl = "/images/artefacts/sannati-buddha-pada.jpg",
                Model3DType = "ashokan_relief"
            },

            // --- SITE 11: SURKOTADA (GUJARAT) ---
            new Artefact
            {
                SiteId = surkotada.Id,
                Name = "Harappan Cast Copper Celt & Flat Chisel",
                ArtefactType = "Metallurgical Woodworking & Masonry Tool",
                Material = "Cast Arsenical Copper / Bronze Alloy",
                ApproximateYear = -2100,
                Dimensions = "Length 14.2 cm, Cutting Edge 7.5 cm",
                Description = "Heavy cast copper celt with a splayed working edge and flat butt, utilized by Harappan stone masons for dressing rubblestone fortification ramparts.",
                CurrentLocation = "Archaeological Survey of India Collection, Vadodara",
                DiscoveryContext = "Excavated from Period IB occupational stratum inside the Citadel barracks",
                ImageUrl = "/images/artefacts/surkotada-copper-celt.jpg",
                Model3DType = "bronze_chariot"
            },
            new Artefact
            {
                SiteId = surkotada.Id,
                Name = "Polychrome Painted Harappan Ceramic Urn",
                ArtefactType = "Slip-Painted Terracotta Storage Vessel",
                Material = "Wheel-Thrown Fired Silt Clay with Red Slip & Black Pigment",
                ApproximateYear = -2200,
                Dimensions = "Height 34 cm, Rim Diameter 18 cm",
                Description = "Diagnostic globular ceramic urn adorned with painted intersecting circles, pipal leaves, and stylized peacocks characteristic of Gujarat Mature Harappan pottery.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Found in situ within a residential room in the Surkotada Lower Town (J.P. Joshi excavation)",
                ImageUrl = "/images/artefacts/surkotada-painted-pottery.jpg",
                Model3DType = "pottery_amphora"
            },

            // --- SITE 12: INAMGAON (MAHARASHTRA) ---
            new Artefact
            {
                SiteId = inamgaon.Id,
                Name = "Chalcolithic Clay Mother Goddess Figurine",
                ArtefactType = "Ritual Terracotta Anthropomorphic Figurine",
                Material = "Low-fired Burnished Terracotta with Red Slip",
                ApproximateYear = -1300,
                Dimensions = "Height 9.5 cm, Width 4.2 cm",
                Description = "Diagnostic Chalcolithic Jorwe culture clay mother goddess figurine with pinched facial features and flared hips, discovered inside a clay receptacle in an in situ mud house floor.",
                CurrentLocation = "Deccan College Archaeological Museum, Pune",
                DiscoveryContext = "Found sealed inside a two-tiered clay box beneath the floor of House 13 (Jorwe Period)",
                ImageUrl = "/images/artefacts/inamgaon-goddess.jpg",
                Model3DType = "terracotta_goddess"
            },
            new Artefact
            {
                SiteId = inamgaon.Id,
                Name = "Jorwe Culture Painted Red Ware Spouted Vessel",
                ArtefactType = "Diagnostic Chalcolithic Ceramic Vessel",
                Material = "Fine Well-Levigated Clay with Red Slip and Black Geometric Painting",
                ApproximateYear = -1200,
                Dimensions = "Height 21 cm, Diameter 16 cm",
                Description = "Characteristic spouted red ware vessel with tubular spout and painted carination showing stylized deer and geometric zigzags, hallmarks of the Deccan Chalcolithic Jorwe culture.",
                CurrentLocation = "Deccan College Museum, Pune",
                DiscoveryContext = "Excavated from House 38 near the hydraulic diversion dam (Dhavalikar & Sankalia)",
                ImageUrl = "/images/artefacts/inamgaon-pottery.jpg",
                Model3DType = "pottery_amphora"
            },

            // --- SITE 13: MOHENJO-DARO (PAKISTAN) ---
            new Artefact
            {
                SiteId = mohenjodaro.Id,
                Name = "The Dancing Girl of Mohenjo-daro",
                ArtefactType = "Cast Bronze Statuette",
                Material = "Bronze (Lost-Wax / Cire Perdue Casting)",
                ApproximateYear = -2300,
                Dimensions = "Height 10.5 cm, Width 5 cm",
                Description = "World-famous masterpiece depicting a young girl standing with right hand on hip and left arm adorned with 24 bangles, capturing extraordinary naturalism.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Found in 1926 by Ernest Mackay in the HR Area of Mohenjo-daro",
                ImageUrl = "/images/artefacts/dancing-girl.jpg",
                Model3DType = "dancing_girl_bronze"
            },
            new Artefact
            {
                SiteId = mohenjodaro.Id,
                Name = "The Celebrated Priest-King of Mohenjo-daro",
                ArtefactType = "Carved Soapstone / Steatite Bust",
                Material = "Glazed Steatite with Traces of Red Pigment",
                ApproximateYear = -2200,
                Dimensions = "Height 17.5 cm, Width 11 cm",
                Description = "The iconic seated patriarchal figure wearing a fillet headband with circular jewel, armlet, and a mantle draped over the left shoulder decorated with trefoil cloverleaf motifs.",
                CurrentLocation = "National Museum of Pakistan, Karachi",
                DiscoveryContext = "Excavated by Sir John Marshall's team in the DK Area of Mohenjo-daro (1927)",
                ImageUrl = "/images/artefacts/priest-king.jpg",
                Model3DType = "ashokan_relief"
            },
            new Artefact
            {
                SiteId = mohenjodaro.Id,
                Name = "The Pashupati / Proto-Shiva Intaglio Seal",
                ArtefactType = "Square Steatite Stamp Seal",
                Material = "Fine-Grained Pyramidal Steatite",
                ApproximateYear = -2350,
                Dimensions = "3.4 cm x 3.4 cm x 0.8 cm",
                Description = "Masterwork seal depicting a horned three-faced figure seated in a yogic asana posture, surrounded by an elephant, tiger, rhinoceros, water buffalo, and two deer beneath the stool.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Discovered by Ernest Mackay in Block 1, Section DK-G of Mohenjo-daro",
                ImageUrl = "/images/artefacts/pashupati-seal.jpg",
                Model3DType = "seal_cube"
            },

            // --- SITE 14: HARAPPA (PAKISTAN) ---
            new Artefact
            {
                SiteId = harappa.Id,
                Name = "Red Jasper Polished Male Torso",
                ArtefactType = "Carved Stone Statuette",
                Material = "Finely Polished Red Sandstone / Jasper",
                ApproximateYear = -2300,
                Dimensions = "Height 9.3 cm, Width 5.8 cm",
                Description = "Peerless masterpiece of Bronze Age naturalistic anatomical carving excavated at Harappa Mound F, featuring socket drill holes on shoulders and neck for attaching articulated head and arms.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Excavated by Madho Sarup Vats in 1928-29 at Harappa Mound F (Stratum III)",
                ImageUrl = "/images/artefacts/harappa-red-jasper-torso.jpg",
                Model3DType = "ashokan_relief"
            },
            new Artefact
            {
                SiteId = harappa.Id,
                Name = "Cemetery H Painted Funerary Urn with Peacock Motif",
                ArtefactType = "Cinerary Urn with Post-Urban Iconography",
                Material = "Fine Red Ware with Deep Black Slip Painting",
                ApproximateYear = -1800,
                Dimensions = "Height 46 cm, Diameter 38 cm",
                Description = "Diagnostic Late Harappan cinerary urn decorated with flying peacocks carrying soul-effigies within their bellies, bull horns, and celestial stars reflecting transformed eschatology.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Excavated from Cemetery H Stratum I urn burials at Harappa",
                ImageUrl = "/images/artefacts/harappa-cemetery-h-pot.jpg",
                Model3DType = "pottery_amphora"
            },

            // --- SITE 15: UR (IRAQ) ---
            new Artefact
            {
                SiteId = ur.Id,
                Name = "Standard of Ur",
                ArtefactType = "Hollow Wooden Box Narrative Mosaic",
                Material = "Lapis Lazuli, Red Limestone, and Shell set in Bitumen",
                ApproximateYear = -2600,
                Dimensions = "Length 49.5 cm, Height 21.5 cm",
                Description = "Dual-sided narrative mosaic box depicting 'War' and 'Peace'. Shows four-wheeled onager war chariots, bound prisoners, a royal banquet, and long-distance luxury tribute.",
                CurrentLocation = "The British Museum, London",
                DiscoveryContext = "Found in the Royal Cemetery of Ur, tomb PG 779 by Sir Leonard Woolley",
                ImageUrl = "/images/artefacts/standard-of-ur.jpg",
                Model3DType = "cuneiform_tablet"
            },
            new Artefact
            {
                SiteId = ur.Id,
                Name = "The Ram in a Thicket",
                ArtefactType = "Composite Religious Offering Stand",
                Material = "Hammered Gold, Silver, Lapis Lazuli, Shell, and Red Limestone over Wood",
                ApproximateYear = -2500,
                Dimensions = "Height 45.7 cm, Width 30.5 cm",
                Description = "Spectacular composite sculpture depicting a rampant goat upright against a blossoming golden tree, emblematic of Sumerian fertility rites and the deity Dumuzi.",
                CurrentLocation = "The British Museum, London",
                DiscoveryContext = "Found crushed together in the 'Great Death Pit' (PG 1237) of the Royal Cemetery of Ur",
                ImageUrl = "/images/artefacts/ur-ram-in-a-thicket.jpg",
                Model3DType = "gold_armlet"
            },
            new Artefact
            {
                SiteId = ur.Id,
                Name = "Golden Floral Headdress of Queen Puabi",
                ArtefactType = "Royal Gold & Gemstone Mortuary Regalia",
                Material = "Pure Beaten Gold, Lapis Lazuli, and Banded Carnelian",
                ApproximateYear = -2500,
                Dimensions = "Headdress height 38 cm, Choker Length 32 cm",
                Description = "Intricate royal regalia comprised of delicate golden beech leaves, weeping willow ribbons, a magnificent seven-pointed gold flower comb, and lapis lazuli choker beads.",
                CurrentLocation = "Penn Museum, Philadelphia",
                DiscoveryContext = "Recovered in situ directly on the skull of Queen Puabi in intact vaulted tomb PG 800",
                ImageUrl = "/images/artefacts/ur-queen-puabi-headdress.jpg",
                Model3DType = "gold_armlet"
            },

            // --- SITE 16: GIZA NECROPOLIS (EGYPT) ---
            new Artefact
            {
                SiteId = giza.Id,
                Name = "Khufu Royal Cedarwood Solar Barque Ship",
                ArtefactType = "Full-Sized Royal Funerary Vessel",
                Material = "Lebanese Cedar (Cedrus libani) and Cordage Rigging",
                ApproximateYear = -2560,
                Dimensions = "Length 43.4 meters, Beam Width 5.9 meters",
                Description = "The oldest intact royal ship in world archaeology, buried in 1,224 dismantled pieces inside a sealed limestone rock-cut pit at the foot of the Great Pyramid for Khufu's celestial voyage with Ra.",
                CurrentLocation = "Grand Egyptian Museum, Giza",
                DiscoveryContext = "Discovered in 1954 hermetically sealed beneath 41 limestone blocks south of the Great Pyramid",
                ImageUrl = "/images/artefacts/giza-khufu-solar-boat.jpg",
                Model3DType = "bronze_chariot"
            },
            new Artefact
            {
                SiteId = giza.Id,
                Name = "Diorite Statue of King Khafre Enthroned",
                ArtefactType = "Monumental Royal Diorite Sculpture",
                Material = "Rare Dark Diorite-Gneiss with Translucent Veins",
                ApproximateYear = -2520,
                Dimensions = "Height 168 cm, Width 57 cm",
                Description = "Masterpiece of Old Kingdom royal power portraying pharaoh Khafre seated on a lion-throne; behind his headdress, the celestial falcon god Horus envelops the king's neck with protective wings.",
                CurrentLocation = "The Egyptian Museum, Cairo",
                DiscoveryContext = "Found by Auguste Mariette in 1860 inside the well of Khafre's Valley Temple at Giza",
                ImageUrl = "/images/artefacts/giza-khafre-enthroned.jpg",
                Model3DType = "ashokan_relief"
            },
            new Artefact
            {
                SiteId = giza.Id,
                Name = "Graywacke Triad of King Menkaure with Hathor",
                ArtefactType = "High-Relief Royal Divine Triad",
                Material = "Polished Fine-Grained Graywacke / Schist",
                ApproximateYear = -2490,
                Dimensions = "Height 95.5 cm, Width 48 cm",
                Description = "Exquisite high-relief carving depicting King Menkaure wearing the White Crown of Upper Egypt, holding hands with the goddess Hathor and the local Nome deity.",
                CurrentLocation = "Museum of Fine Arts, Boston",
                DiscoveryContext = "Excavated by George Reisner in 1908 inside the Menkaure Valley Temple at Giza",
                ImageUrl = "/images/artefacts/giza-triad-menkaure.jpg",
                Model3DType = "ashokan_relief"
            },

            // --- SITE 17: KNOSSOS (GREECE) ---
            new Artefact
            {
                SiteId = knossos.Id,
                Name = "Minoan Faience Snake Goddess Figurine",
                ArtefactType = "Votive Palatial Cult Figurine",
                Material = "Glazed Polychrome Faience",
                ApproximateYear = -1600,
                Dimensions = "Height 34.2 cm",
                Description = "Iconic statuette of a Minoan priestess or deity wearing a flounced tiered skirt, tightly laced bodice exposing breasts, and brandishing writhing snakes in both outstretched hands.",
                CurrentLocation = "Heraklion Archaeological Museum, Crete",
                DiscoveryContext = "Excavated by Sir Arthur Evans in 1903 from the stone Temple Repositories in the West Wing",
                ImageUrl = "/images/artefacts/knossos-snake-goddess.jpg",
                Model3DType = "terracotta_goddess"
            },
            new Artefact
            {
                SiteId = knossos.Id,
                Name = "Bull-Leaping Palace Wall Fresco",
                ArtefactType = "Palatial Polychrome Lime Plaster Fresco",
                Material = "Wet Lime Plaster (Buon Fresco) with Mineral Pigments",
                ApproximateYear = -1450,
                Dimensions = "Height 80 cm, Length 140 cm",
                Description = "Vivid palatial fresco depicting the acrobatic Minoan ritual of taurokathapsia: an athlete somersaulting over the back of a galloping bull flanked by two female attendants.",
                CurrentLocation = "Heraklion Archaeological Museum, Crete",
                DiscoveryContext = "Excavated from the upper court wall debris in the East Wing of the Palace of Knossos",
                ImageUrl = "/images/artefacts/knossos-bull-leaping-fresco.jpg",
                Model3DType = "cave_art_slab"
            },
            new Artefact
            {
                SiteId = knossos.Id,
                Name = "Monumental Ceramic Pithos Storage Oil Jars",
                ArtefactType = "Palatial Bulk Oil & Wine Storage Vessel",
                Material = "Heavy Kiln-Fired Coarse Terracotta with Rope Relief",
                ApproximateYear = -1550,
                Dimensions = "Height 180 cm, Capacity approx. 500 liters each",
                Description = "Colossal storage jars decorated with raised rope-work designs and multiple suspension handles, lining the West Magazines to store thousands of liters of olive oil and wine.",
                CurrentLocation = "In situ at the West Magazines, Palace of Knossos",
                DiscoveryContext = "Uncovered along the corridors of the West Magazines during Sir Arthur Evans's initial excavations",
                ImageUrl = "/images/artefacts/knossos-pithoi-jar.jpg",
                Model3DType = "pottery_amphora"
            },

            // --- SITE 18: POMPEII (ITALY) ---
            new Artefact
            {
                SiteId = pompeii.Id,
                Name = "The Pompeii Indian Ivory Statuette of Lakshmi / Yakshi",
                ArtefactType = "Carved Elephant Ivory Statuette",
                Material = "Indian Elephant Ivory with Intricate Bas-Relief",
                ApproximateYear = 50,
                Dimensions = "Height 25 cm",
                Description = "Sensational proof of direct maritime commerce between ancient India and the Roman Empire: an Indian ivory statuette of a female deity/yakshi discovered in Pompeii, buried by Vesuvius in 79 CE.",
                CurrentLocation = "National Archaeological Museum of Naples (MANN)",
                DiscoveryContext = "Unearthed in October 1938 inside a wooden jewel box in the Casa dei Quattro Stili (Regio I, Insula 8)",
                ImageUrl = "/images/artefacts/pompeii-lakshmi.jpg",
                Model3DType = "ashokan_relief"
            },
            new Artefact
            {
                SiteId = pompeii.Id,
                Name = "The Alexander Mosaic from the House of the Faun",
                ArtefactType = "Monumental Opus Vermiculatum Floor Mosaic",
                Material = "Over 1.5 million hand-cut polychrome stone and glass tesserae",
                ApproximateYear = -100,
                Dimensions = "5.82 meters x 3.13 meters",
                Description = "World-famous masterpiece depicting the climactic Battle of Issus (333 BCE) between Alexander the Great mounted on Bucephalus and Persian King Darius III in his royal chariot.",
                CurrentLocation = "National Archaeological Museum of Naples (MANN)",
                DiscoveryContext = "Excavated in 1831 in the exedra room between the peristyles of the House of the Faun",
                ImageUrl = "/images/artefacts/pompeii-alexander-mosaic.jpg",
                Model3DType = "cave_art_slab"
            },
            new Artefact
            {
                SiteId = pompeii.Id,
                Name = "Fresco Portrait of a Young Woman with Stylus ('Sappho')",
                ArtefactType = "Domestic Wall Fresco (Fourth Pompeian Style)",
                Material = "Polychrome Pigments on Smooth Hydraulic Lime Plaster",
                ApproximateYear = 55,
                Dimensions = "Diameter 37 cm",
                Description = "Iconic tondo fresco depicting an educated upper-class Roman woman holding a four-leaf wax tablet (polyptych) and pressing a bronze stylus against her lips in thoughtful contemplation.",
                CurrentLocation = "National Archaeological Museum of Naples (MANN)",
                DiscoveryContext = "Excavated in June 1760 from Regio VI, Insula 17 at Pompeii",
                ImageUrl = "/images/artefacts/pompeii-fresco-sappho.jpg",
                Model3DType = "cave_art_slab"
            },

            // --- SITE 19: PYRAMIDS OF MEROË (SUDAN) ---
            new Artefact
            {
                SiteId = meroe.Id,
                Name = "Golden Armlet of Queen Amanishakheto",
                ArtefactType = "Royal Gold Cloisonné Armlet",
                Material = "Solid Gold with Enamel and Glass Inlay",
                ApproximateYear = -10,
                Dimensions = "Height 7.5 cm, Diameter 8.2 cm",
                Description = "A magnificent royal armlet portraying the winged goddess Hathor or Mut protecting the Candace (Kushite Queen) Amanishakheto, excavated from Pyramid Beg. N. 6.",
                CurrentLocation = "Egyptian Museum of Berlin & State Museum of Egyptian Art, Munich",
                DiscoveryContext = "Found in 1834 inside the burial chamber of Pyramid Beg. N. 6 at Meroë",
                ImageUrl = "/images/artefacts/meroe-armlet.jpg",
                Model3DType = "gold_armlet"
            },
            new Artefact
            {
                SiteId = meroe.Id,
                Name = "Meroë Bronze Head of Roman Emperor Augustus",
                ArtefactType = "Cast Bronze Imperial Portrait",
                Material = "Hollow-Cast Bronze with Alabaster and Glass Eyes",
                ApproximateYear = -25,
                Dimensions = "Height 46.2 cm, Weight 16.5 kg",
                Description = "Looted by Kushite armies during Queen Amanirenas's raid on Roman Upper Egypt in 24 BCE and buried deliberately beneath the steps of a victory temple in Meroë so worshippers trampled Caesar's face.",
                CurrentLocation = "The British Museum, London",
                DiscoveryContext = "Excavated in 1910 by John Garstang beneath the entryway steps of Temple M292 at Meroë",
                ImageUrl = "/images/artefacts/meroe-head-augustus.jpg",
                Model3DType = "ashokan_relief"
            },

            // --- SITE 20: PETRA (JORDAN) ---
            new Artefact
            {
                SiteId = petra.Id,
                Name = "Nabataean Painted Fine Ware Bowl",
                ArtefactType = "Eggshell-Thin Painted Ceramic",
                Material = "Kiln-Fired Terracotta with Natural Mineral Slip",
                ApproximateYear = 50,
                Dimensions = "Diameter 18.5 cm, Height 5.2 cm",
                Description = "Ultra-thin luxury bowl (under 2 mm wall thickness) painted with stylized palmettes and peacock feathers, hallmark of Nabataean ceramic mastery.",
                CurrentLocation = "Petra Archaeological Museum, Jordan",
                DiscoveryContext = "Excavated from the residential quarter near the Colonnaded Street",
                ImageUrl = "/images/artefacts/petra-bowl.jpg",
                Model3DType = "sangam_potsherd"
            },
            new Artefact
            {
                SiteId = petra.Id,
                Name = "Sandstone Eye Idol of Goddess Atargatis / Al-Uzza",
                ArtefactType = "Aniconic Cult Stele (Betyl)",
                Material = "Local Rose Sandstone with Carved Facial Features",
                ApproximateYear = 20,
                Dimensions = "Height 38 cm, Width 22 cm, Thickness 8 cm",
                Description = "Distinctive Nabataean aniconic rectangular stele featuring large stylized geometric eyes and nose representing the supreme Arabian goddess Al-Uzza or Atargatis.",
                CurrentLocation = "Petra Archaeological Museum, Jordan",
                DiscoveryContext = "Recovered from the Temple of the Winged Lions on the northern ridge of Petra",
                ImageUrl = "/images/artefacts/petra-eye-idol.jpg",
                Model3DType = "stone_stele"
            },

            // --- SITE 21: MACHU PICCHU (PERU) ---
            new Artefact
            {
                SiteId = machuPicchu.Id,
                Name = "Inca Ceremonial Bronze Tumi Knife",
                ArtefactType = "Ritual Sacrificial Blade",
                Material = "Tin Bronze (Andean Alloy)",
                ApproximateYear = 1470,
                Dimensions = "Length 16 cm, Width 11 cm",
                Description = "Crescent-bladed ceremonial knife surmounted by an anthropomorphic solar priest figure, used in Inti Raymi solar festival rituals.",
                CurrentLocation = "Museo Larco, Lima, Peru",
                DiscoveryContext = "Recovered from high-status tomb assemblage in the Sacred Plaza sector",
                ImageUrl = "/images/artefacts/machu-picchu-tumi.jpg",
                Model3DType = "bronze_chariot"
            },
            new Artefact
            {
                SiteId = machuPicchu.Id,
                Name = "Classic Inca Polychrome Geometric Aryballos Ceramic Jar",
                ArtefactType = "Imperial Chicha Fermentation Vessel",
                Material = "Fine Andean Clay with Red, Black, and Ochre Mineral Slip",
                ApproximateYear = 1480,
                Dimensions = "Height 48 cm, Diameter 32 cm",
                Description = "Conical-based transport amphora adorned with geometric fern patterns and a jaguar-head lug handle, engineered to be carried across mountain trails with a back-strap.",
                CurrentLocation = "Museo Machu Picchu Casa Concha, Cusco",
                DiscoveryContext = "Excavated from the residential sector of the Inca nobility by Hiram Bingham's Yale expedition",
                ImageUrl = "/images/artefacts/machu-picchu-aryballos.jpg",
                Model3DType = "pottery_amphora"
            },
            new Artefact
            {
                SiteId = machuPicchu.Id,
                Name = "Inca Dyed Knotted Cotton Cord Quipu Record",
                ArtefactType = "Tawantinsuyu Decimal Accounting Device",
                Material = "Spun and Pled Cotton and Alpaca Fiber with Vegetable Dyes",
                ApproximateYear = 1500,
                Dimensions = "Main cord length 65 cm, Pendant cords 45 cm",
                Description = "A primary administrative instrument using color-coded cords and clustered decimal knots to record tribute, granary stores, and population censuses across the Inca Empire.",
                CurrentLocation = "Museo Larco, Lima, Peru",
                DiscoveryContext = "Found sealed inside a stone niche in the Royal Estate administrative sector",
                ImageUrl = "/images/artefacts/machu-picchu-quipu.jpg",
                Model3DType = "gold_armlet"
            },

            // --- SITE 22: STONEHENGE & AVEBURY (UNITED KINGDOM) ---
            new Artefact
            {
                SiteId = stonehenge.Id,
                Name = "Bush Barrow Gold Lozenge",
                ArtefactType = "Ceremonial Hammered Gold Breastplate",
                Material = "Beaten Sheet Gold with Geometric Tooling",
                ApproximateYear = -1900,
                Dimensions = "Length 18.4 cm, Width 15.6 cm",
                Description = "A diamond-shaped sheet of pure gold adorned with exquisite repeating geometric zigzags and borders, embodying advanced Bronze Age astronomy and geometry.",
                CurrentLocation = "Wiltshire Museum, Devizes, UK",
                DiscoveryContext = "Found in 1808 resting on the chest of a Bronze Age chieftain in Bush Barrow",
                ImageUrl = "/images/artefacts/stonehenge-lozenge.jpg",
                Model3DType = "ashokan_relief"
            },
            new Artefact
            {
                SiteId = stonehenge.Id,
                Name = "Red Deer Antler Excavation Pick from Ditch Strata",
                ArtefactType = "Neolithic Earthworking Megalithic Tool",
                Material = "Red Deer (Cervus elaphus) Antler Tine",
                ApproximateYear = -3000,
                Dimensions = "Length 52 cm",
                Description = "Diagnostic Neolithic bone tool with battered tines used to dig the circular ditch and chalk banks of Stonehenge Phase 1; prime organic source for high-precision radiocarbon dating.",
                CurrentLocation = "Salisbury Museum, Wiltshire",
                DiscoveryContext = "Recovered directly from the primary chalk silt at the bottom of the Stonehenge outer ditch",
                ImageUrl = "/images/artefacts/stonehenge-antler-pick.jpg",
                Model3DType = "terracotta_tablet"
            },

            // --- SITE 23: ANGKOR WAT (CAMBODIA) ---
            new Artefact
            {
                SiteId = angkorWat.Id,
                Name = "Stone Portrait Head of King Jayavarman VII (Bayon Style)",
                ArtefactType = "Sandstone Sculptural Portrait",
                Material = "Carved Fine-Grained Greenish-Grey Sandstone",
                ApproximateYear = 1190,
                Dimensions = "Height 42 cm, Width 26 cm",
                Description = "Diagnostic Late Angkorian masterpiece from the National Museum of Cambodia portraying the great Buddhist monarch Jayavarman VII with serene half-closed meditative eyes, smiling lips, and coiled hair ushnisha.",
                CurrentLocation = "National Museum of Cambodia, Phnom Penh",
                DiscoveryContext = "Recovered from the Angkor royal enclosure during archaeological clearance",
                ImageUrl = "/images/artefacts/angkor-wat-avalokiteshvara.jpg",
                Model3DType = "ashokan_relief"
            },
            new Artefact
            {
                SiteId = angkorWat.Id,
                Name = "Bas-Relief of the Churning of the Ocean of Milk",
                ArtefactType = "Continuous Monolithic Wall Bas-Relief",
                Material = "Phnom Kulen Fine Sandstone",
                ApproximateYear = 1140,
                Dimensions = "Panel Length 49 meters, Height 2.2 meters",
                Description = "The monumental east gallery relief depicting 88 Asuras and 92 Devas churning the cosmic milk ocean using the serpent Vasuki wrapped around Mount Mandara, overseen by four-armed Vishnu.",
                CurrentLocation = "In situ on the third enclosure eastern gallery wall of Angkor Wat",
                DiscoveryContext = "Commissioned by King Suryavarman II as the theological centerpiece of Angkor Wat",
                ImageUrl = "/images/artefacts/angkor-wat-churning-milk.jpg",
                Model3DType = "ashokan_relief"
            },
            new Artefact
            {
                SiteId = angkorWat.Id,
                Name = "Historic Relief of King Suryavarman II in Royal Procession",
                ArtefactType = "Imperial Historical Stone Relief",
                Material = "Carved Sandstone",
                ApproximateYear = 1135,
                Dimensions = "Height 2.0 meters, Width 3.5 meters",
                Description = "Splendid relief depicting King Suryavarman II seated under fifteen royal parasols and peacock feather fans holding the royal battle-axe, commanding his army.",
                CurrentLocation = "In situ at the South Gallery western wing, Angkor Wat",
                DiscoveryContext = "Identified by Henri Mouhot during his 1860 documentation of Angkor Wat's bas-relief series",
                ImageUrl = "/images/artefacts/angkor-wat-suryavarman.jpg",
                Model3DType = "ashokan_relief"
            },

            // --- SITE 24: COLOSSEUM & ROMAN FORUM (ITALY) ---
            new Artefact
            {
                SiteId = colosseum.Id,
                Name = "Imperial Gladiatorial Murmillo Helmet & Sica",
                ArtefactType = "Embossed Bronze Ceremonial Armor & Curved Blade",
                Material = "Tinned Bronze & Forged Iron",
                ApproximateYear = 80,
                Dimensions = "Helmet Height 48 cm, Weight 3.8 kg",
                Description = "Heavy ceremonial helmet with a broad brim, perforated visor grates, and crest embossed with a marine monster, alongside a curved sica blade.",
                CurrentLocation = "National Roman Museum, Palazzo Massimo, Rome",
                DiscoveryContext = "Excavated from the gladiator barracks (Ludus Magnus) adjacent to the Colosseum",
                ImageUrl = "/images/artefacts/gladiator-helmet.jpg",
                Model3DType = "bronze_chariot"
            },
            new Artefact
            {
                SiteId = colosseum.Id,
                Name = "Bronze Sestertius of Emperor Titus Depicting the Colosseum",
                ArtefactType = "Imperial Commemorative Coinage",
                Material = "Struck Orichalcum / Bronze",
                ApproximateYear = 80,
                Dimensions = "Diameter 34.5 mm, Weight 24.8 grams",
                Description = "Celebrated imperial coin struck in 80 CE to commemorate the grand opening games of the Amphitheatrum Flavium, showing the tiered exterior arcades filled with statues, packed spectators, and the Meta Sudans fountain.",
                CurrentLocation = "British Museum, London & Capitoline Museums, Rome",
                DiscoveryContext = "Excavated from the Flavian destruction horizon in the Roman Forum",
                ImageUrl = "/images/artefacts/colosseum-sestertius.jpg",
                Model3DType = "terracotta_tablet"
            }
        );

        // ==========================================
        // 8. EXCAVATIONS & STRATIGRAPHY (WHEELER-BOX MATRICES)
        // ==========================================
        var excDholavira = new Excavation
        {
            SiteId = dholavira.Id,
            ExpeditionName = "Dholavira Systematic Scientific Project",
            LeadArchaeologist = "Dr. Ravindra Singh Bisht",
            StartYear = 1990,
            EndYear = 2005,
            Organization = "Archaeological Survey of India (Excavation Branch V)",
            Summary = "Fifteen seasons of deep soundings uncovering 7 continuous cultural stages from pre-Harappan formative settlement through mature planning to post-urban decline."
        };

        var excKeeladi = new Excavation
        {
            SiteId = keeladi.Id,
            ExpeditionName = "Vaigai River Valley Scientific Archaeological Project",
            LeadArchaeologist = "Dr. K. Amarnath Ramakrishna & Dr. R. Sivanantham",
            StartYear = 2014,
            EndYear = 2026,
            Organization = "Archaeological Survey of India & Tamil Nadu State Department of Archaeology",
            Summary = "Multi-phase deep soundings uncovering 6th century BCE urban layers, brick channels, ring wells, and literate Sangam society."
        };

        var excHarappa = new Excavation
        {
            SiteId = harappa.Id,
            ExpeditionName = "Harappa Archaeological Research Project (HARP)",
            LeadArchaeologist = "Dr. Jonathan Mark Kenoyer & Dr. Richard H. Meadow",
            StartYear = 1986,
            EndYear = 2010,
            Organization = "University of Wisconsin-Madison & Department of Archaeology, Pakistan",
            Summary = "Modern multidisciplinary stratigraphic excavation using micro-morphology, paleobotany, and AMS dating to establish the five-period chronological sequence of the Indus tradition."
        };

        var excMohenjo = new Excavation
        {
            SiteId = mohenjodaro.Id,
            ExpeditionName = "Mohenjo-daro Deep Sounding & Urban Survey Expeditions",
            LeadArchaeologist = "Sir John Marshall, Ernest Mackay & Sir Mortimer Wheeler",
            StartYear = 1922,
            EndYear = 1965,
            Organization = "Archaeological Survey of India & UNESCO International Campaign",
            Summary = "Monumental clearances uncovering the Citadel platform, Great Bath, HR and DK domestic quarters, and deep soundings reaching waterlogged basal layers."
        };

        var excLothal = new Excavation
        {
            SiteId = lothal.Id,
            ExpeditionName = "Lothal Maritime Settlement Excavations",
            LeadArchaeologist = "Dr. Shikaripura Ranganatha Rao",
            StartYear = 1955,
            EndYear = 1962,
            Organization = "Archaeological Survey of India",
            Summary = "Seven excavation seasons revealing the baked-brick tidal dockyard, warehouse acropolis, and micro-bead lapidary industrial factory."
        };

        var excPompeii = new Excavation
        {
            SiteId = pompeii.Id,
            ExpeditionName = "Pompeii Stratigraphic & Volcanic Tephra Project",
            LeadArchaeologist = "Dr. Amedeo Maiuri & Soprintendenza Archeologica di Pompei",
            StartYear = 1924,
            EndYear = 2026,
            Organization = "Parco Archeologico di Pompei & University Consortia",
            Summary = "Systematic excavations of the 79 CE pyroclastic surge horizons, insulae domestic architecture, and pre-Roman Samnite stratigraphic sequences."
        };

        var excKnossos = new Excavation
        {
            SiteId = knossos.Id,
            ExpeditionName = "Palace of Minos Stratigraphic Exploration",
            LeadArchaeologist = "Sir Arthur Evans & British School at Athens",
            StartYear = 1900,
            EndYear = 1935,
            Organization = "British School at Athens",
            Summary = "Excavations establishing the tripartite Minoan chronological framework (Early, Middle, Late Minoan) based on ceramic seriation and palatial architectural horizons."
        };

        var excUr = new Excavation
        {
            SiteId = ur.Id,
            ExpeditionName = "Ur of the Chaldees Joint Expedition",
            LeadArchaeologist = "Sir C. Leonard Woolley",
            StartYear = 1922,
            EndYear = 1934,
            Organization = "British Museum & University of Pennsylvania Museum",
            Summary = "Twelve landmark seasons uncovering the Ziggurat complex, 1,800 graves in the Royal Cemetery, and the deep 'Flood Stratum' silt."
        };


        var excRakhigarhi = new Excavation
        {
            SiteId = rakhigarhi.Id,
            ExpeditionName = "Rakhigarhi Paleogenetic & Urban Metropolis Project",
            LeadArchaeologist = "Prof. Vasant Shinde & Dr. Amarendra Nath",
            StartYear = 1997,
            EndYear = 2016,
            Organization = "Archaeological Survey of India & Deccan College (Pune)",
            Summary = "Extensive soundings across mounds RGR-1 to RGR-7 uncovering multi-room mudbrick architecture, granaries, lapidary bead workshops, and cemetery paleogenetic sampling."
        };

        var excKalibangan = new Excavation
        {
            SiteId = kalibangan.Id,
            ExpeditionName = "Kalibangan Ghaggar Valley Archaeological Project",
            LeadArchaeologist = "Prof. B. B. Lal, B. K. Thapar & J. P. Joshi",
            StartYear = 1960,
            EndYear = 1969,
            Organization = "Archaeological Survey of India",
            Summary = "Decade of landmark soundings discovering the world's earliest criss-cross ploughed agricultural field (Period I) and fortified citadel with ritual fire altars (Period II)."
        };

        var excSinauli = new Excavation
        {
            SiteId = sinauli.Id,
            ExpeditionName = "Sinauli Royal Necropolis & Warrior Chariot Excavation",
            LeadArchaeologist = "Dr. Sanjay Kumar Manjul & Arvin Manjul",
            StartYear = 2018,
            EndYear = 2019,
            Organization = "Archaeological Survey of India (Excavation Branch II)",
            Summary = "Sensational discovery of 2000 BCE royal warrior burials: solid-wheeled war chariots adorned with copper triangles, antennae swords with wire hilts, and anthropomorphic copper sheets."
        };

        var excBhimbetka = new Excavation
        {
            SiteId = bhimbetka.Id,
            ExpeditionName = "Bhimbetka Rock Shelters Stratigraphic Survey",
            LeadArchaeologist = "Dr. Vishnu Shridhar Wakankar & Dr. V. N. Misra",
            StartYear = 1973,
            EndYear = 1977,
            Organization = "Vikram University (Ujjain) & Deccan College (Pune)",
            Summary = "Stratigraphic trial trenches in Shelter III F-23 revealing continuous occupational deposits from Acheulian Lower Paleolithic through Mesolithic rock art painting phases."
        };

        var excArikamedu = new Excavation
        {
            SiteId = arikamedu.Id,
            ExpeditionName = "Arikamedu Indo-Roman Port Excavations",
            LeadArchaeologist = "Sir R. E. Mortimer Wheeler & J.-M. Casal",
            StartYear = 1945,
            EndYear = 1950,
            Organization = "Archaeological Survey of India & Mission Archéologique Française",
            Summary = "Classic Wheeler-box stratigraphic soundings linking imported Roman Arretine terra sigillata ware and Mediterranean wine amphorae to Indian Megalithic Black-and-Red ware."
        };

        var excSannati = new Excavation
        {
            SiteId = sannati.Id,
            ExpeditionName = "Kanaganahalli (Sannati) Mahastupa Excavation",
            LeadArchaeologist = "Dr. K. P. Poonacha & Dr. D. V. Devaraj",
            StartYear = 1994,
            EndYear = 2002,
            Organization = "Archaeological Survey of India (Bangalore Circle)",
            Summary = "Unearthing of the monumental Adholoka Mahachaitya stupa, 60 inscribed Ashokan limestone slabs, and the only known sculpted portrait of Emperor Ashoka inscribed 'Raya Asoka'."
        };

        var excPataliputra = new Excavation
        {
            SiteId = pataliputra.Id,
            ExpeditionName = "Kumrahar & Bulandibagh Mauryan Capital Excavations",
            LeadArchaeologist = "Dr. David Brainard Spooner & Dr. L. A. Waddell",
            StartYear = 1912,
            EndYear = 1927,
            Organization = "Archaeological Survey of India",
            Summary = "Deep alluvial silt soundings uncovering the monolithic 80-pillared Mauryan hypostyle assembly hall and double-timber defensive palisade ramparts described by Megasthenes."
        };

        var excSurkotada = new Excavation
        {
            SiteId = surkotada.Id,
            ExpeditionName = "Surkotada Citadel & Fortification Excavation",
            LeadArchaeologist = "Jagat Pati Joshi",
            StartYear = 1971,
            EndYear = 1972,
            Organization = "Archaeological Survey of India (Excavation Branch)",
            Summary = "Stratigraphic excavation of an intact rubblestone Harappan citadel and residential annex showing three continuous occupational sub-periods (IA, IB, IC) with copper celts."
        };

        var excInamgaon = new Excavation
        {
            SiteId = inamgaon.Id,
            ExpeditionName = "Inamgaon Chalcolithic Settlement & Hydrology Project",
            LeadArchaeologist = "Prof. M. K. Dhavalikar, H. D. Sankalia & Z. D. Ansari",
            StartYear = 1968,
            EndYear = 1982,
            Organization = "Deccan College Post-Graduate and Research Institute (Pune)",
            Summary = "Fourteen extensive seasons uncovering 130 mud houses, an engineered irrigation canal and embankment, painted Jorwe spouted ware, and mother goddess terracotta cults."
        };

        var excGiza = new Excavation
        {
            SiteId = giza.Id,
            ExpeditionName = "Giza Plateau Mapping Project & Pyramid Builders City",
            LeadArchaeologist = "Dr. Mark Lehner & Dr. Zahi Hawass",
            StartYear = 1988,
            EndYear = 2026,
            Organization = "Ancient Egypt Research Associates (AERA) & Ministry of Tourism and Antiquities",
            Summary = "Detailed stratigraphic excavations of Heit el-Ghurab (Lost City of the Pyramid Builders), worker bakeries, cattle corrals, and 4th Dynasty royal mortuary causeways."
        };

        var excMeroe = new Excavation
        {
            SiteId = meroe.Id,
            ExpeditionName = "Meroë Royal City & Kushite Necropolis Expedition",
            LeadArchaeologist = "Prof. John Garstang & UNESCO / University of Khartoum",
            StartYear = 1909,
            EndYear = 1914,
            Organization = "University of Liverpool & National Corporation for Antiquities and Museums",
            Summary = "Excavations of the steep-angled royal pyramid cemeteries of Kushite kings and Candaces, the Royal Baths, and monumental bloomery iron smelting slag mounds."
        };

        var excPetra = new Excavation
        {
            SiteId = petra.Id,
            ExpeditionName = "Petra Great Temple & Siq Hydraulic Excavations",
            LeadArchaeologist = "Dr. Martha Sharp Joukowsky & Dr. Philip C. Hammond",
            StartYear = 1993,
            EndYear = 2008,
            Organization = "Brown University & Department of Antiquities of Jordan",
            Summary = "Fifteen seasons of stratigraphic excavation uncovering the Great Temple, Nabataean paved street, pressurized ceramic water piping, and rock-cut sanctuaries."
        };

        var excMachu = new Excavation
        {
            SiteId = machuPicchu.Id,
            ExpeditionName = "Machu Picchu Paleohydrology & Architecture Project",
            LeadArchaeologist = "Dr. Kenneth R. Wright & Alfredo Valencia Zegarra",
            StartYear = 1994,
            EndYear = 2005,
            Organization = "Instituto Nacional de Cultura (INC Peru) & Wright Paleohydrological Institute",
            Summary = "Sub-surface trenching examining the Inca subsurface drainage system, 16 cascaded ceremonial stone fountains, agricultural terrace filtration, and cyclopean masonry."
        };

        var excStonehenge = new Excavation
        {
            SiteId = stonehenge.Id,
            ExpeditionName = "Stonehenge Riverside Project & Ditch Stratigraphy",
            LeadArchaeologist = "Prof. Michael Parker Pearson & Prof. Richard J. C. Atkinson",
            StartYear = 1950,
            EndYear = 2009,
            Organization = "English Heritage & Universities Consortium (Sheffield/Manchester/UCL)",
            Summary = "Stratigraphic soundings across the outer circular ditch and Aubrey Holes, recovering in situ Neolithic red deer antler excavation picks and radiocarbon dating Phase 1-3."
        };

        var excAngkor = new Excavation
        {
            SiteId = angkorWat.Id,
            ExpeditionName = "Greater Angkor Project & Hydraulic Network Survey",
            LeadArchaeologist = "Dr. Roland Fletcher, Dr. Christophe Pottier & EFEO",
            StartYear = 2000,
            EndYear = 2020,
            Organization = "University of Sydney, APSARA National Authority & École française d'Extrême-Orient",
            Summary = "Comprehensive stratigraphic soundings and LiDAR surveys tracing the monumental sand-and-laterite temple foundations and the 1,000 sq km hydraulic canal network."
        };

        var excColosseum = new Excavation
        {
            SiteId = colosseum.Id,
            ExpeditionName = "Colosseum Hypogeum & Subterranean Chamber Clearance",
            LeadArchaeologist = "Dr. Heinz-Jürgen Beste & Dr. Rossella Rea",
            StartYear = 1996,
            EndYear = 2002,
            Organization = "Parco Archeologico del Colosseo & German Archaeological Institute (DAI)",
            Summary = "Multi-year clearance and architectural stratigraphy of the subterranean Flavian arena hypogeum, revealing elevator hoist shafts, trap doors, and hydraulic draining conduits."
        };

        context.Excavations.AddRange(
            excDholavira, excKeeladi, excHarappa, excMohenjo, excLothal, excPompeii, excKnossos, excUr,
            excRakhigarhi, excKalibangan, excSinauli, excBhimbetka, excArikamedu, excSannati, excPataliputra,
            excSurkotada, excInamgaon, excGiza, excMeroe, excPetra, excMachu, excStonehenge, excAngkor, excColosseum
        );
        await context.SaveChangesAsync();

        // --- Stratigraphic Layers ---
        // Dholavira Strata
        var layerD1 = new ExcavationLayer
        {
            ExcavationId = excDholavira.Id,
            LayerNumber = 1,
            LayerName = "Stage VII: Late Post-Urban Encampment",
            DepthMeters = 0.6,
            SoilComposition = "10YR 7/3 (Very Pale Brown) loose wind-blown sand, aeolian deposit and crumbling rubble",
            EstimatedStartYear = -1650,
            EstimatedEndYear = -1500,
            CulturalAffiliation = "Late Harappan (Jhukar-like ceramic affinity)",
            Description = "Impoverished sub-urban circular stone hut structures without urban drainage, weights, or writing."
        };
        var layerD2 = new ExcavationLayer
        {
            ExcavationId = excDholavira.Id,
            LayerNumber = 2,
            LayerName = "Stage IV-V: Peak Mature Harappan Urban Horizon",
            DepthMeters = 2.4,
            SoilComposition = "10YR 5/4 (Yellowish Brown) dense compacted occupational floor with burnt lime plaster and paved brick",
            EstimatedStartYear = -2500,
            EstimatedEndYear = -2000,
            CulturalAffiliation = "Mature Harappan (Classic Indus)",
            Description = "Apex of architectural monumentalism: tripartite stone citadel walls, rock-cut reservoirs, standardized Indus seals, weights, and the monumental Signboard."
        };
        var layerD3 = new ExcavationLayer
        {
            ExcavationId = excDholavira.Id,
            LayerNumber = 3,
            LayerName = "Stage I: Early Pre-Harappan Settlement",
            DepthMeters = 6.8,
            SoilComposition = "7.5YR 4/4 (Brown) sterile weathered bedrock overlain with riverine silt and non-Harappan red slip pottery",
            EstimatedStartYear = -3000,
            EstimatedEndYear = -2600,
            CulturalAffiliation = "Early Pre-Harappan",
            Description = "First stone and mudbrick fortification walls built directly over bed-rock; wheel-made bichrome and monochrome pottery."
        };

        // Keeladi Strata
        var layerK1 = new ExcavationLayer
        {
            ExcavationId = excKeeladi.Id,
            LayerNumber = 1,
            LayerName = "Stratum IV: Early Historic Sangam Horizon (AMS 580 BCE)",
            DepthMeters = 2.8,
            SoilComposition = "10YR 3/2 (Very Dark Grayish Brown) compacted dark alluvial clay with burnt brick fragments, charcoal nodules, and pot sherds",
            EstimatedStartYear = -600,
            EstimatedEndYear = -300,
            CulturalAffiliation = "Early Sangam (Old Tamil / Tamil-Brahmi)",
            Description = "Continuous occupational stratum yielding Tamil-Brahmi inscribed Black-and-Red ware potsherds, ring wells, and lapidary carnelian bead debitage."
        };
        var layerK2 = new ExcavationLayer
        {
            ExcavationId = excKeeladi.Id,
            LayerNumber = 2,
            LayerName = "Stratum II: Industrial Weaving & Roman Commerce Horizon",
            DepthMeters = 1.2,
            SoilComposition = "10YR 5/3 (Brown) silty sand with brick kilns, terracotta drain pipes, and glass slag",
            EstimatedStartYear = -200,
            EstimatedEndYear = 200,
            CulturalAffiliation = "Mature Sangam Era",
            Description = "Industrial quarter containing open brick dye vats, spindle whorls, gold filigree ornaments, and imported Arretine ceramic sherds."
        };

        // Harappa Strata
        var layerH1 = new ExcavationLayer
        {
            ExcavationId = excHarappa.Id,
            LayerNumber = 1,
            LayerName = "Period 5: Cemetery H Horizon",
            DepthMeters = 1.0,
            SoilComposition = "7.5YR 6/4 (Light Brown) alluvial loam with red-and-black burial urn clusters",
            EstimatedStartYear = -1900,
            EstimatedEndYear = -1300,
            CulturalAffiliation = "Late Harappan (Cemetery H Culture)",
            Description = "Funerary urn burials marked by peacock eschatological motifs; decline of monumental civic granaries."
        };
        var layerH2 = new ExcavationLayer
        {
            ExcavationId = excHarappa.Id,
            LayerNumber = 2,
            LayerName = "Period 3C: Peak Mature Harappan Metropolis",
            DepthMeters = 2.8,
            SoilComposition = "10YR 6/3 (Pale Brown) compact brick debris, street silt, and drainage sediment",
            EstimatedStartYear = -2450,
            EstimatedEndYear = -2000,
            CulturalAffiliation = "Mature Harappan (Harappa Phase)",
            Description = "Maximum urban extent: Mound AB Citadel ramparts, circular grain threshing floors, red jasper male torso statuary, and intaglio unicorn seals."
        };
        var layerH3 = new ExcavationLayer
        {
            ExcavationId = excHarappa.Id,
            LayerNumber = 3,
            LayerName = "Period 1: Ravi Phase (Early Formative Settlement)",
            DepthMeters = 7.2,
            SoilComposition = "10YR 4/2 (Dark Grayish Brown) virgin clay overlain with early hearths and bone debris",
            EstimatedStartYear = -3300,
            EstimatedEndYear = -2800,
            CulturalAffiliation = "Ravi Phase (Hakra Ware Horizon)",
            Description = "Earliest agro-pastoral village settlement, handmade polychrome pottery, bone tools, and proto-script potter marks incised before firing."
        };

        // Mohenjo-daro Strata
        var layerM1 = new ExcavationLayer
        {
            ExcavationId = excMohenjo.Id,
            LayerNumber = 1,
            LayerName = "Late Period: Post-Urban Sub-Division Phase",
            DepthMeters = 1.4,
            SoilComposition = "10YR 7/2 (Light Gray) wind-blown dust, collapsing brick kilns, and flood silt",
            EstimatedStartYear = -1900,
            EstimatedEndYear = -1700,
            CulturalAffiliation = "Late Harappan Decline",
            Description = "Encroachment of courtyard houses onto public streets, partitioned rooms, and unburied skeletons in HR Area."
        };
        var layerM2 = new ExcavationLayer
        {
            ExcavationId = excMohenjo.Id,
            LayerNumber = 2,
            LayerName = "Intermediate Period II: Great Bath & Acropolis Peak",
            DepthMeters = 3.6,
            SoilComposition = "10YR 5/2 (Grayish Brown) kiln-fired brick masonry with bitumen waterproofing and gypsiferous mortar",
            EstimatedStartYear = -2400,
            EstimatedEndYear = -2100,
            CulturalAffiliation = "Mature Harappan Zenith",
            Description = "Erection of the bitumen-waterproofed Great Bath, the pillared assembly hall, and recovered masterpieces including the Priest-King and Dancing Girl."
        };

        // Lothal Strata
        var layerL1 = new ExcavationLayer
        {
            ExcavationId = excLothal.Id,
            LayerNumber = 1,
            LayerName = "Phase II-IV: Engineered Tidal Dockyard & Industrial Acropolis",
            DepthMeters = 2.5,
            SoilComposition = "10YR 6/2 (Light Brownish Gray) marine estuary silt with brick paving and kiln slag",
            EstimatedStartYear = -2350,
            EstimatedEndYear = -2000,
            CulturalAffiliation = "Mature Harappan Maritime Horizon",
            Description = "Operation of the 214m brick tidal basin, lock-gate sluice, warehouse acropolis, and mass production of micro-drilled carnelian beads for Mesopotamian export."
        };

        // Pompeii Strata
        var layerP1 = new ExcavationLayer
        {
            ExcavationId = excPompeii.Id,
            LayerNumber = 1,
            LayerName = "79 CE Vesuvius Pyroclastic Pumice & Lapilli Fallout",
            DepthMeters = 3.2,
            SoilComposition = "5Y 8/1 (White) volcanic pumice tephra overlain with dark pyroclastic ash surge flow",
            EstimatedStartYear = 79,
            EstimatedEndYear = 79,
            CulturalAffiliation = "Flavian Roman Imperial",
            Description = "Hermetic volcanic burial sealing domestic villas, wall frescoes, the Alexander Mosaic, and luxury exotic imports including the Indian ivory Lakshmi statuette."
        };

        // Knossos Strata
        var layerKn1 = new ExcavationLayer
        {
            ExcavationId = excKnossos.Id,
            LayerNumber = 1,
            LayerName = "Late Minoan I-II: Neopalatial Fresco & Pithoi Horizon",
            DepthMeters = 2.1,
            SoilComposition = "10YR 6/4 (Light Yellowish Brown) crushed limestone debris, gypsum slabs, and plaster fragments",
            EstimatedStartYear = -1600,
            EstimatedEndYear = -1450,
            CulturalAffiliation = "Neopalatial Minoan",
            Description = "Peak of Minoan palatial splendor: Bull-Leaping frescoes, West Magazines with 500-liter pithoi jars, and faience Snake Goddess figurines."
        };

        // Ur Strata
        var layerU1 = new ExcavationLayer
        {
            ExcavationId = excUr.Id,
            LayerNumber = 1,
            LayerName = "Early Dynastic III-A: Royal Cemetery Necropolis",
            DepthMeters = 5.4,
            SoilComposition = "10YR 4/3 (Brown) clay loam with bitumen brick coffins and gold burial offerings",
            EstimatedStartYear = -2600,
            EstimatedEndYear = -2500,
            CulturalAffiliation = "Early Dynastic Sumerian",
            Description = "Vaulted stone burial chambers yielding the Standard of Ur, Ram in a Thicket, and Queen Puabi's gold floral headdress."
        };


        // Rakhigarhi Strata
        var layerR1 = new ExcavationLayer
        {
            ExcavationId = excRakhigarhi.Id,
            LayerNumber = 1,
            LayerName = "Period III: Peak Mature Harappan Urban Metropolis",
            DepthMeters = 2.4,
            SoilComposition = "10YR 5/3 (Brown) compact clay floor with paved baked-brick drains and hearths",
            EstimatedStartYear = -2500,
            EstimatedEndYear = -1900,
            CulturalAffiliation = "Mature Harappan",
            Description = "Planned mudbrick houses, public granary storage structures, lapidary workshops, and steatite unicorn seals."
        };
        var layerR2 = new ExcavationLayer
        {
            ExcavationId = excRakhigarhi.Id,
            LayerNumber = 2,
            LayerName = "Period II: Early Harappan Sothi-Siswal Settlement",
            DepthMeters = 4.8,
            SoilComposition = "10YR 4/2 (Dark Grayish Brown) dense mudbrick debris and charcoal ash lenses",
            EstimatedStartYear = -3300,
            EstimatedEndYear = -2600,
            CulturalAffiliation = "Early Harappan (Sothi-Siswal)",
            Description = "Mudbrick houses on standardized ratios, bichrome painted pottery, bone awls, and copper chisel fragments."
        };

        // Kalibangan Strata
        var layerKb1 = new ExcavationLayer
        {
            ExcavationId = excKalibangan.Id,
            LayerNumber = 1,
            LayerName = "Period II: Mature Harappan Fortified Citadel & Fire Altars",
            DepthMeters = 1.8,
            SoilComposition = "7.5YR 5/4 (Strong Brown) clay brick masonry with ritual terracotta cakes and ash pits",
            EstimatedStartYear = -2600,
            EstimatedEndYear = -1900,
            CulturalAffiliation = "Mature Harappan",
            Description = "Citadel with 7 clay-lined ritual fire altars containing ash and bovine bones; rectilinear street grid with sanitary drains."
        };
        var layerKb2 = new ExcavationLayer
        {
            ExcavationId = excKalibangan.Id,
            LayerNumber = 2,
            LayerName = "Period I: Early Harappan Ploughed Field Horizon",
            DepthMeters = 3.9,
            SoilComposition = "10YR 5/2 (Grayish Brown) alluvial silt preserving criss-cross agricultural furrow casts",
            EstimatedStartYear = -2900,
            EstimatedEndYear = -2600,
            CulturalAffiliation = "Early Harappan (Kalibangan I)",
            Description = "The world's earliest excavated ploughed field showing criss-cross furrows for dual-cropping of mustard and horsegram."
        };

        // Sinauli Strata
        var layerSn1 = new ExcavationLayer
        {
            ExcavationId = excSinauli.Id,
            LayerNumber = 1,
            LayerName = "Burial Trench B3: Royal Anthropomorphic Sarcophagus Horizon",
            DepthMeters = 1.4,
            SoilComposition = "10YR 4/3 (Dark Brown) silty sand with copper corrosion patination staining",
            EstimatedStartYear = -1900,
            EstimatedEndYear = -1800,
            CulturalAffiliation = "Copper Hoard Warrior Elite",
            Description = "Royal wooden coffins (manjushas) decorated with anthropomorphic copper sheets, solid-wheeled war chariots, and antennae swords."
        };
        var layerSn2 = new ExcavationLayer
        {
            ExcavationId = excSinauli.Id,
            LayerNumber = 2,
            LayerName = "Habitation Trench H1: Late Ochre Coloured Pottery (OCP) Floor",
            DepthMeters = 2.2,
            SoilComposition = "7.5YR 4/4 (Reddish Brown) compact clay with charcoal fragments and smelting crucibles",
            EstimatedStartYear = -2100,
            EstimatedEndYear = -1900,
            CulturalAffiliation = "Late OCP / Early Copper Age",
            Description = "Domestic living floor yielding copper flat chisels, pottery kilns, and steatite paste beads."
        };

        // Bhimbetka Strata
        var layerBh1 = new ExcavationLayer
        {
            ExcavationId = excBhimbetka.Id,
            LayerNumber = 1,
            LayerName = "Period III: Mesolithic Microlithic & Pigment Workshop Floor",
            DepthMeters = 0.6,
            SoilComposition = "5YR 3/3 (Dark Reddish Brown) soil rich in hematite crayons, charcoals, and quartzite chips",
            EstimatedStartYear = -8000,
            EstimatedEndYear = -3000,
            CulturalAffiliation = "Mesolithic Hunter-Gatherer",
            Description = "Hematite grinding stones, geometric microliths (trapezes, lunates), and pigment preparation tools used on Zoo Rock."
        };
        var layerBh2 = new ExcavationLayer
        {
            ExcavationId = excBhimbetka.Id,
            LayerNumber = 2,
            LayerName = "Period I: Acheulian Lower Paleolithic Quartzite Floor",
            DepthMeters = 2.8,
            SoilComposition = "10YR 3/4 (Dark Yellowish Brown) lateritic gravel with compact quartzite rubble",
            EstimatedStartYear = -100000,
            EstimatedEndYear = -40000,
            CulturalAffiliation = "Acheulian Lower Paleolithic",
            Description = "Deepest occupational layer yielding massive quartzite Acheulian handaxes, cleavers, and scrapers."
        };

        // Arikamedu Strata
        var layerAr1 = new ExcavationLayer
        {
            ExcavationId = excArikamedu.Id,
            LayerNumber = 1,
            LayerName = "Wheeler Stratum II: Roman Ceramic & Amphorae Horizon",
            DepthMeters = 1.8,
            SoilComposition = "10YR 6/2 (Light Brownish Gray) estuarine sandy clay with brick fragments",
            EstimatedStartYear = -50,
            EstimatedEndYear = 100,
            CulturalAffiliation = "Indo-Roman Global Maritime Trade",
            Description = "Abundant fragments of Mediterranean Dressel 2-4 wine amphorae with resin linings, Roman Arretine terra sigillata, and blue glass beads."
        };

        // Sannati Strata
        var layerSa1 = new ExcavationLayer
        {
            ExcavationId = excSannati.Id,
            LayerNumber = 1,
            LayerName = "Upper Medhi: Satavahana Sculptural Casing Slabs",
            DepthMeters = 1.2,
            SoilComposition = "10YR 6/3 (Pale Brown) calcareous rubble with broken limestone slabs",
            EstimatedStartYear = 50,
            EstimatedEndYear = 250,
            CulturalAffiliation = "Satavahana Buddhist Horizon",
            Description = "Adornment of the Mahastupa medhi with intricately carved limestone slabs depicting the life of Buddha and Emperor Ashoka."
        };
        var layerSa2 = new ExcavationLayer
        {
            ExcavationId = excSannati.Id,
            LayerNumber = 2,
            LayerName = "Lower Core: Mauryan Ashokan Stupa Foundation",
            DepthMeters = 3.2,
            SoilComposition = "10YR 4/3 (Brown) rammed morrum and river pebble platform",
            EstimatedStartYear = -260,
            EstimatedEndYear = -200,
            CulturalAffiliation = "Mauryan Imperial (Ashokan)",
            Description = "Original brick stupa core and inscribed granite slabs containing Special Rock Edicts XII & XIV in Mauryan Brahmi script."
        };

        // Pataliputra Strata
        var layerPt1 = new ExcavationLayer
        {
            ExcavationId = excPataliputra.Id,
            LayerNumber = 1,
            LayerName = "Spooner Stratum IV: Mauryan 80-Pillared Hypostyle Hall",
            DepthMeters = 5.2,
            SoilComposition = "10YR 3/2 (Very Dark Grayish Brown) Gangetic alluvial silt over a thick layer of charcoal and burnt sal wood",
            EstimatedStartYear = -300,
            EstimatedEndYear = -185,
            CulturalAffiliation = "Mauryan Imperial Court",
            Description = "Ashoka's colossal 80-pillared audience hall with mirror-polished Chunar sandstone shafts resting on monolithic stone base blocks."
        };
        var layerPt2 = new ExcavationLayer
        {
            ExcavationId = excPataliputra.Id,
            LayerNumber = 2,
            LayerName = "Bulandibagh Trench: Imperial Teakwood Palisade Ramparts",
            DepthMeters = 6.8,
            SoilComposition = "10YR 2/2 (Very Dark Brown) waterlogged anaerobic marsh clay preserving timber",
            EstimatedStartYear = -320,
            EstimatedEndYear = -250,
            CulturalAffiliation = "Early Mauryan (Chandragupta)",
            Description = "Double row of massive teakwood uprights joined by heavy beams, forming the monumental imperial city rampart described by Megasthenes."
        };

        // Surkotada Strata
        var layerSk1 = new ExcavationLayer
        {
            ExcavationId = excSurkotada.Id,
            LayerNumber = 1,
            LayerName = "Sub-Period IC: Late Harappan Rubblestone Rebuilding",
            DepthMeters = 0.9,
            SoilComposition = "10YR 6/4 (Light Yellowish Brown) rubble stones with White-Painted Black-and-Red Ware",
            EstimatedStartYear = -1950,
            EstimatedEndYear = -1700,
            CulturalAffiliation = "Late Harappan (Surkotada IC)",
            Description = "Reconstruction of citadel gateways with coarse rubble masonry; flat copper chisels, beads, and bone tools."
        };
        var layerSk2 = new ExcavationLayer
        {
            ExcavationId = excSurkotada.Id,
            LayerNumber = 2,
            LayerName = "Sub-Period IA: Mature Harappan Citadel & Residential Annex",
            DepthMeters = 3.2,
            SoilComposition = "10YR 5/3 (Brown) compact mud mortar, mudbrick slabs, and chert blades",
            EstimatedStartYear = -2300,
            EstimatedEndYear = -1950,
            CulturalAffiliation = "Mature Harappan (Classic Indus)",
            Description = "Massive rubble-and-mudbrick ramparts with defensive bastions, copper celts, steatite micro-beads, and painted Indus pottery."
        };

        // Inamgaon Strata
        var layerIn1 = new ExcavationLayer
        {
            ExcavationId = excInamgaon.Id,
            LayerNumber = 1,
            LayerName = "Late Jorwe Phase: Round Mud Huts & Spouted Ware",
            DepthMeters = 0.8,
            SoilComposition = "10YR 4/2 (Dark Grayish Brown) black cotton soil with pottery sherds",
            EstimatedStartYear = -1000,
            EstimatedEndYear = -700,
            CulturalAffiliation = "Late Jorwe Culture",
            Description = "Clusters of circular mud huts, channel-spouted red ware pots, decline of agriculture and increased pastoral reliance."
        };
        var layerIn2 = new ExcavationLayer
        {
            ExcavationId = excInamgaon.Id,
            LayerNumber = 2,
            LayerName = "Early Jorwe Phase: Rectangular Houses & Hydraulic Canal",
            DepthMeters = 2.1,
            SoilComposition = "10YR 3/3 (Dark Brown) silty clay floor with lime wash and storage pits",
            EstimatedStartYear = -1400,
            EstimatedEndYear = -1000,
            CulturalAffiliation = "Early Jorwe Culture",
            Description = "Prosperous settlement of 130 rectangular multi-room houses, massive stone irrigation embankment, and painted Jorwe spouted ware."
        };

        // Giza Strata
        var layerGz1 = new ExcavationLayer
        {
            ExcavationId = excGiza.Id,
            LayerNumber = 1,
            LayerName = "Heit el-Ghurab: Lost City of the Pyramid Builders",
            DepthMeters = 2.2,
            SoilComposition = "10YR 7/3 (Pale Brown) desert sand overlying mudbrick worker dormitories",
            EstimatedStartYear = -2550,
            EstimatedEndYear = -2450,
            CulturalAffiliation = "Old Kingdom 4th Dynasty",
            Description = "Planned municipal town for pyramid workers: institutional bakeries producing emmer bread, cattle bone processing, and copper craft workshops."
        };

        // Meroe Strata
        var layerMr1 = new ExcavationLayer
        {
            ExcavationId = excMeroe.Id,
            LayerNumber = 1,
            LayerName = "Begarawiyah North: Royal Pyramid Necropolis Horizon",
            DepthMeters = 3.2,
            SoilComposition = "10YR 6/4 (Light Yellowish Brown) sandstone rubble and wind-blown Nubian sand",
            EstimatedStartYear = -300,
            EstimatedEndYear = 100,
            CulturalAffiliation = "Kingdom of Kush (Meroitic)",
            Description = "Steep-angled sandstone royal pyramids with mortuary chapels and pylon gates, yielding gold armlets and Hellenistic imports."
        };

        // Petra Strata
        var layerPtN1 = new ExcavationLayer
        {
            ExcavationId = excPetra.Id,
            LayerNumber = 1,
            LayerName = "Nabataean Imperial Horizon: Al-Khazneh & Colonnaded Street",
            DepthMeters = 2.4,
            SoilComposition = "7.5YR 6/4 (Reddish Yellow) weathered rose-red sandstone sand and paved limestone flagstones",
            EstimatedStartYear = -100,
            EstimatedEndYear = 106,
            CulturalAffiliation = "Nabataean Classical Zenith",
            Description = "Monumental rock-cut temple façades, terracotta pressurized water conduits, and eggshell-thin painted Nabataean bowls."
        };

        // Machu Picchu Strata
        var layerMp1 = new ExcavationLayer
        {
            ExcavationId = excMachu.Id,
            LayerNumber = 1,
            LayerName = "Hanan (Upper) Urban Sector: Royal Estate & Drainage Stratum",
            DepthMeters = 1.2,
            SoilComposition = "10YR 3/1 (Very Dark Gray) rich organic topsoil over granite sub-surface drainage rubble",
            EstimatedStartYear = 1450,
            EstimatedEndYear = 1540,
            CulturalAffiliation = "Imperial Inca (Pachacuti)",
            Description = "Cyclopean dry-stone granite palaces, 16 cascaded ceremonial stone fountains, agricultural terraces, and bronze tumi knives."
        };

        // Stonehenge Strata
        var layerSh1 = new ExcavationLayer
        {
            ExcavationId = excStonehenge.Id,
            LayerNumber = 1,
            LayerName = "Phase 1: Outer Ditch & Aubrey Holes Primary Chalk Silt",
            DepthMeters = 2.6,
            SoilComposition = "10YR 8/1 (White) virgin compact chalk silt at the base of the circular ditch",
            EstimatedStartYear = -3000,
            EstimatedEndYear = -2800,
            CulturalAffiliation = "Early Neolithic Britain",
            Description = "Basal chalk ditch deposits containing in situ battered red deer antler excavation picks and ox scapulae used as shovels."
        };

        // Angkor Wat Strata
        var layerAw1 = new ExcavationLayer
        {
            ExcavationId = excAngkor.Id,
            LayerNumber = 1,
            LayerName = "Central Temple Sanctuary & Sand Hydraulic Foundation",
            DepthMeters = 2.1,
            SoilComposition = "10YR 4/2 (Dark Grayish Brown) compacted alluvial sand-clay under laterite core blocks",
            EstimatedStartYear = 1113,
            EstimatedEndYear = 1150,
            CulturalAffiliation = "Classical Khmer Empire (Suryavarman II)",
            Description = "Engineered sand foundation engineered to retain moisture and stabilize massive sandstone towers; continuous gallery wall bas-reliefs."
        };

        // Colosseum Strata
        var layerCl1 = new ExcavationLayer
        {
            ExcavationId = excColosseum.Id,
            LayerNumber = 1,
            LayerName = "Flavian Hypogeum Subterranean Stage Mechanics Horizon",
            DepthMeters = 3.5,
            SoilComposition = "10YR 6/2 (Light Brownish Gray) travertine dust and pozzolanic hydraulic concrete sediment",
            EstimatedStartYear = 80,
            EstimatedEndYear = 200,
            CulturalAffiliation = "Flavian Roman Imperial",
            Description = "Subterranean masonry channels with counterweight elevator hoists, wild animal cage shafts, and bronze gladiatorial equipment."
        };

        context.ExcavationLayers.AddRange(
            layerD1, layerD2, layerD3,
            layerK1, layerK2,
            layerH1, layerH2, layerH3,
            layerM1, layerM2,
            layerL1, layerP1, layerKn1, layerU1,
            layerR1, layerR2,
            layerKb1, layerKb2,
            layerSn1, layerSn2,
            layerBh1, layerBh2,
            layerAr1,
            layerSa1, layerSa2,
            layerPt1, layerPt2,
            layerSk1, layerSk2,
            layerIn1, layerIn2,
            layerGz1,
            layerMr1,
            layerPtN1,
            layerMp1,
            layerSh1,
            layerAw1,
            layerCl1
        );
        await context.SaveChangesAsync();

        context.Findings.AddRange(
            new Finding
            {
                ExcavationLayerId = layerD2.Id,
                Name = "The Dholavira Ten-Glyph Municipal Signboard",
                FindingType = "Monumental Inscription",
                Description = "White crystalline gypsum letters fallen face down in the Western Gateway",
                YearFound = 1990
            },
            new Finding
            {
                ExcavationLayerId = layerK1.Id,
                Name = "Potsherd Inscribed with 'Aadhan' in Tamil-Brahmi",
                FindingType = "Inscribed Ceramic",
                Description = "Black-and-Red Ware rim sherd recovered from charcoal-dated 580 BCE stratum",
                YearFound = 2018
            },
            new Finding
            {
                ExcavationLayerId = layerH2.Id,
                Name = "Red Jasper Male Anatomical Torso",
                FindingType = "Sculpture",
                Description = "Polished red sandstone torso found in Mound F Stratum III",
                YearFound = 1928
            },
            new Finding
            {
                ExcavationLayerId = layerM2.Id,
                Name = "Bronze Dancing Girl Statuette",
                FindingType = "Cast Bronze Metalwork",
                Description = "Lost-wax cast bronze statuette recovered from the HR Area residential court",
                YearFound = 1926
            },
            new Finding
            {
                ExcavationLayerId = layerL1.Id,
                Name = "Persian Gulf Steatite Button Seal",
                FindingType = "Trade Stamp Seal",
                Description = "Circular glazed steatite seal found adjacent to the tidal dock basin",
                YearFound = 1958
            },
            new Finding
            {
                ExcavationLayerId = layerP1.Id,
                Name = "Indian Carved Ivory Lakshmi Statuette",
                FindingType = "Imported Exotic Adornment",
                Description = "Ivory statuette recovered inside a jewel box in Casa dei Quattro Stili",
                YearFound = 1938
            },
            new Finding
            {
                ExcavationLayerId = layerKn1.Id,
                Name = "Faience Snake Goddess Figurine",
                FindingType = "Ritual Cult Figurine",
                Description = "Glazed faience figurine from the stone cists of the Temple Repositories",
                YearFound = 1903
            },
            new Finding
            {
                ExcavationLayerId = layerU1.Id,
                Name = "The Standard of Ur Mosaic Box",
                FindingType = "Narrative Mosaic Box",
                Description = "Double-sided shell and lapis lazuli mosaic from royal tomb PG 779",
                YearFound = 1927
            },
            new Finding
            {
                ExcavationLayerId = layerR1.Id,
                Name = "Terracotta Spoked Wheel Model & Unicorn Seal",
                FindingType = "Glyptic Intaglio & Toy Cart",
                Description = "In situ Mature Harappan toy cart wheel and square glazed steatite seal from Mound RGR-2",
                YearFound = 2014
            },
            new Finding
            {
                ExcavationLayerId = layerKb2.Id,
                Name = "Criss-Cross Agricultural Ploughed Furrows",
                FindingType = "Agricultural Feature",
                Description = "Intact grid pattern of ancient ploughed furrows preserved under sand dunes",
                YearFound = 1968
            },
            new Finding
            {
                ExcavationLayerId = layerSn1.Id,
                Name = "Royal Solid-Wheeled War Chariot Chassis",
                FindingType = "Martial Vehicle",
                Description = "Full-sized two-wheeled war chariot with embossed copper triangles from burial trench 8",
                YearFound = 2018
            },
            new Finding
            {
                ExcavationLayerId = layerBh1.Id,
                Name = "Hematite Pigment Crayon with Faceted Wear",
                FindingType = "Pigment Processing Tool",
                Description = "Natural iron-oxide crayon ground down against quartzite rock for shelter paintings",
                YearFound = 1974
            },
            new Finding
            {
                ExcavationLayerId = layerSa1.Id,
                Name = "Inscribed 'Raya Asoka' Limestone Portrait Slab",
                FindingType = "Imperial Sculptural Relief",
                Description = "Carved relief showing Emperor Ashoka flanked by royal consorts, inscribed in Brahmi script",
                YearFound = 1997
            },
            new Finding
            {
                ExcavationLayerId = layerPt1.Id,
                Name = "Mirror-Polished Chunar Sandstone Pillar Capital",
                FindingType = "Architectural Monument",
                Description = "Monolithic polished column fragment with Hellenistic palmette and honeysuckle motifs",
                YearFound = 1913
            },
            new Finding
            {
                ExcavationLayerId = layerSk2.Id,
                Name = "Harappan Cast Copper Celt and Chisel",
                FindingType = "Metallurgical Implement",
                Description = "Cast copper celt with flattened butt and splayed cutting edge from Citadel Trench 2",
                YearFound = 1971
            },
            new Finding
            {
                ExcavationLayerId = layerIn2.Id,
                Name = "Jorwe Culture Painted Red Ware Spouted Pot",
                FindingType = "Ceramic Vessel",
                Description = "Fine wheel-made red slipped vessel with tubular spout and painted black deer motif",
                YearFound = 1972
            },
            new Finding
            {
                ExcavationLayerId = layerSh1.Id,
                Name = "Red Deer Antler Excavation Pick",
                FindingType = "Neolithic Organic Tool",
                Description = "Antler pick with battered tines found resting directly on virgin chalk bedrock",
                YearFound = 1953
            },
            new Finding
            {
                ExcavationLayerId = layerPtN1.Id,
                Name = "Sandstone Eye Idol of Goddess Atargatis",
                FindingType = "Religious Votive Stela",
                Description = "Carved sandstone stela with stylized facial features and inset staring eyes",
                YearFound = 1999
            }
        );

        // ==========================================
        // 9. ARCHAEOLOGICAL RELATIONSHIPS
        // ==========================================
        context.SiteRelationships.AddRange(
            new SiteRelationship
            {
                SourceSiteId = lothal.Id,
                TargetSiteId = dholavira.Id,
                RelationshipType = "Maritime & Inland Trade Corridor",
                Description = "Lothal operated as the coastal port and carnelian lapidary manufacturing depot supplying processed luxury beads to Dholavira's transit gateway."
            },
            new SiteRelationship
            {
                SourceSiteId = rakhigarhi.Id,
                TargetSiteId = harappa.Id,
                RelationshipType = "Northern Metropolitan Twin Axis",
                Description = "Rakhigarhi and Harappa dominated the eastern and central channels of the Indus-Ghaggar river system with identical weights, brick ratios, and granary designs."
            },
            new SiteRelationship
            {
                SourceSiteId = kalibangan.Id,
                TargetSiteId = rakhigarhi.Id,
                RelationshipType = "Ghaggar-Hakra Agrarian Network",
                Description = "Kalibangan provided advanced agricultural surplus (dual-furrow cropping) along the Ghaggar-Hakra watercourse connecting to Rakhigarhi."
            },
            new SiteRelationship
            {
                SourceSiteId = keeladi.Id,
                TargetSiteId = arikamedu.Id,
                RelationshipType = "Sangam Coromandel Coastal & Maritime Trade Network",
                Description = "Keeladi's interior gem-working and textile production fed directly into the international maritime export wharves of Arikamedu."
            },
            new SiteRelationship
            {
                SourceSiteId = arikamedu.Id,
                TargetSiteId = pompeii.Id,
                RelationshipType = "Indo-Roman Trans-Oceanic Commerce",
                Description = "Direct historical trade link evidenced by Mediterranean amphorae at Arikamedu and Indian ivory statuettes discovered preserved in Pompeii."
            },
            new SiteRelationship
            {
                SourceSiteId = pataliputra.Id,
                TargetSiteId = sannati.Id,
                RelationshipType = "Mauryan Imperial Highway & Edict Administration",
                Description = "Imperial edicts issued from the court at Pataliputra were transported to Sannati and carved onto granite slabs and stupas under royal decree."
            },
            new SiteRelationship
            {
                SourceSiteId = lothal.Id,
                TargetSiteId = ur.Id,
                RelationshipType = "Direct Long-Distance Maritime Commerce (Meluhha-Mesopotamia)",
                Description = "Documented by Persian Gulf seals and cuneiform trade records referencing seafaring merchants of Meluhha bringing carnelian beads and copper ingots into the port of Ur."
            },
            new SiteRelationship
            {
                SourceSiteId = meroe.Id,
                TargetSiteId = giza.Id,
                RelationshipType = "Nile River Valley Kushite-Egyptian Cultural & Imperial Corridor",
                Description = "Meroë's royal kings and Candaces maintained deep spiritual reverence for Egyptian religious traditions while developing their own distinct pyramid architecture and Meroitic script."
            },
            new SiteRelationship
            {
                SourceSiteId = petra.Id,
                TargetSiteId = arikamedu.Id,
                RelationshipType = "Nabataean-Indian Ocean Maritime & Overland Incense Highway",
                Description = "Nabataean merchants controlled the Arabian caravan nodes that met trans-oceanic Indian merchant vessels arriving with pepper, cinnamon, and malabathrum."
            },
            new SiteRelationship
            {
                SourceSiteId = arikamedu.Id,
                TargetSiteId = colosseum.Id,
                RelationshipType = "Direct Indo-Roman Imperial Luxury Maritime Silk & Spice Network",
                Description = "Documented by the Periplus of the Erythraean Sea, linking southern Indian ports with the Roman capital where pepper, silk, and exotic animals were consumed in vast quantities."
            }
        );

        await context.SaveChangesAsync();
    }
}
