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

        context.References.AddRange(refBisht, refRao, refShinde, refLal, refManjul, refWakankar, refAmarnath, refWheeler, refPoonacha, refMarshall, refKenoyer, refWoolley, refLehner, refEvans, refBeard);
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
            DatingPrecision = "Radiocarbon (14C) AMS & Stratigraphic Horizons (Stages I-VII)",
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
            DatingPrecision = "Radiocarbon (14C) & Stratigraphic Phases I-V",
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
            DatingPrecision = "Radiocarbon AMS & Ancient Genome Sequencing (Cell 2019)",
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
            DatingPrecision = "Stratigraphic Sequence & 14C Radiocarbon",
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
            DatingPrecision = "AMS Radiocarbon & Archaeological Magnetometry",
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
            DatingPrecision = "Optically Stimulated Luminescence (OSL) & Superimposed Pictorial Stratigraphy",
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
            DatingPrecision = "Stratigraphic Sequence, Epigraphy & Greek Historical Synchronization",
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
            DatingPrecision = "AMS Radiocarbon Dating (Beta Analytic, Miami) & Epigraphic Tamil-Brahmi",
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
            DatingPrecision = "Roman Terra Sigillata Potter Stamps & Radiocarbon",
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
            DatingPrecision = "Mauryan & Satavahana Epigraphy, Paleography & Radiocarbon",
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
            DatingPrecision = "Radiocarbon (14C) Sequence Phases IA, IB, IC",
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
            DatingPrecision = "Radiocarbon (14C) & Ceramic Seriation",
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
            DatingPrecision = "Radiocarbon (14C) & Stratigraphic Deep Soundings",
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
            DatingPrecision = "Radiocarbon (14C) stratigraphic calibration by HARP",
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
            DatingPrecision = "Cuneiform Epigraphy, King Lists, and Radiocarbon",
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
            DatingPrecision = "Astronomical alignments, Quarry worker graffiti & Radiocarbon",
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
            DatingPrecision = "Stratigraphic Ceramic Sequence (EM, MM, LM) & Cross-Dating with Egyptian Dynasties",
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
            DatingPrecision = "Historical accounts (Pliny the Younger) & Volcanic tephrochronology (79 CE)",
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
            DatingPrecision = "Radiocarbon AMS dating & Kushite royal stelae",
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
            DatingPrecision = "Epigraphic Aramaic inscriptions & ceramic typologies",
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
            DatingPrecision = "Carbon-14 dating of human remains and ceramics (Inca Imperial Horizon)",
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
            DatingPrecision = "Multi-phase radiocarbon dates of antler picks and cremated human remains",
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
            DatingPrecision = "Inscriptions of Suryavarman II & epigraphic chronologies",
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
            DatingPrecision = "Extensive epigraphy, classical historical records (Tacitus, Suetonius, Cassius Dio)",
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
            new SiteReference { SiteId = dholavira.Id, ReferenceId = refBisht.Id, SpecificPagesOrPlates = "pp. 45-120; Plates XII-XXX" },
            new SiteReference { SiteId = lothal.Id, ReferenceId = refRao.Id, SpecificPagesOrPlates = "Vol I, pp. 23-88; Dockyard analysis" },
            new SiteReference { SiteId = rakhigarhi.Id, ReferenceId = refShinde.Id, SpecificPagesOrPlates = "Cell 179(3), pp. 729-735; Ancient DNA I6113" },
            new SiteReference { SiteId = kalibangan.Id, ReferenceId = refLal.Id, SpecificPagesOrPlates = "pp. 67-142; Furrow agriculture & Fire altars" },
            new SiteReference { SiteId = sinauli.Id, ReferenceId = refManjul.Id, SpecificPagesOrPlates = "Puratattva 50, pp. 1-25; Solid-wheel war chariots" },
            new SiteReference { SiteId = bhimbetka.Id, ReferenceId = refWakankar.Id, SpecificPagesOrPlates = "pp. 12-65; Zoo Rock & Mesolithic pigments" },
            new SiteReference { SiteId = keeladi.Id, ReferenceId = refAmarnath.Id, SpecificPagesOrPlates = "pp. 1-84; 6th century BCE Tamil-Brahmi script" },
            new SiteReference { SiteId = arikamedu.Id, ReferenceId = refWheeler.Id, SpecificPagesOrPlates = "Ancient India No. 2, pp. 17-124; Roman amphorae" },
            new SiteReference { SiteId = sannati.Id, ReferenceId = refPoonacha.Id, SpecificPagesOrPlates = "pp. 45-98; Inscribed portrait of Emperor Ashoka" },
            new SiteReference { SiteId = mohenjodaro.Id, ReferenceId = refMarshall.Id, SpecificPagesOrPlates = "Vol I, Chapter 3: The Great Bath" },
            new SiteReference { SiteId = harappa.Id, ReferenceId = refKenoyer.Id, SpecificPagesOrPlates = "pp. 55-92; Mound AB and Cemetery R-37" },
            new SiteReference { SiteId = ur.Id, ReferenceId = refWoolley.Id, SpecificPagesOrPlates = "pp. 12-45; Royal Tombs PG 789" },
            new SiteReference { SiteId = giza.Id, ReferenceId = refLehner.Id, SpecificPagesOrPlates = "pp. 108-133; Great Pyramid of Khufu" },
            new SiteReference { SiteId = knossos.Id, ReferenceId = refEvans.Id, SpecificPagesOrPlates = "Vol I, pp. 200-245; The Central Court" },
            new SiteReference { SiteId = pompeii.Id, ReferenceId = refBeard.Id, SpecificPagesOrPlates = "pp. 75-102; Street life and domestic housing" }
        );

        // ==========================================
        // 7. DIAGNOSTIC ARTEFACTS (WITH 3D SHADER MODEL TYPES)
        // ==========================================
        context.Artefacts.AddRange(
            new Artefact
            {
                SiteId = dholavira.Id,
                Name = "The Dholavira Inscription (The Signboard)",
                ArtefactType = "Monumental Inscribed Signboard",
                Material = "Crystalline White Gypsum inlaid in wooden board",
                ApproximateYear = -2300,
                Dimensions = "Letters approx. 37 cm high each, board approx. 3 meters long",
                Description = "A set of ten monumental Indus symbols discovered in a room adjoining the Western Gateway of the Citadel. The gypsum characters originally inlaid in cedar wood represent one of the world's oldest municipal signboards.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Discovered fallen face-down inside the Western Gateway of the Dholavira Citadel (Trench K-8)",
                ImageUrl = "/images/artefacts/dholavira-signboard.jpg",
                Model3DType = "stone_stele"
            },
            new Artefact
            {
                SiteId = lothal.Id,
                Name = "Persian Gulf Steatite Button Seal",
                ArtefactType = "Circular Compartmented Stamp Seal",
                Material = "Glazed Steatite with Intaglio Carving",
                ApproximateYear = -2100,
                Dimensions = "Diameter 2.25 cm, Thickness 0.6 cm",
                Description = "A circular steatite button seal with two jumping ibexes flanking a sun motif, diagnostic of maritime trade with Dilmun (Bahrain) and Ur in Mesopotamia.",
                CurrentLocation = "Archaeological Museum, Lothal",
                DiscoveryContext = "Found in the Warehouse area near the Tidal Dockyard basin (SR Rao excavation)",
                ImageUrl = "/images/artefacts/lothal-seal.jpg",
                Model3DType = "seal_cube"
            },
            new Artefact
            {
                SiteId = rakhigarhi.Id,
                Name = "Indus Intaglio Unicorn Seal & Agate Beads",
                ArtefactType = "Square Intaglio Stamp Seal & Micro-Beads",
                Material = "Low-fired Steatite and Banded Agate",
                ApproximateYear = -2500,
                Dimensions = "Seal 3.2 cm x 3.2 cm",
                Description = "Finely carved seal showing a sacred unicorn before a standard/incense burner, accompanied by 5 Indus pictographs, alongside high-precision micro-drilled banded agate beads.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Found in Mound 2 residential workshop area",
                ImageUrl = "/images/artefacts/rakhigarhi-unicorn-seal.jpg",
                Model3DType = "seal_cube"
            },
            new Artefact
            {
                SiteId = kalibangan.Id,
                Name = "Terracotta Sacrificial Havana Cake & Fire Vessel",
                ArtefactType = "Ritual Terracotta Cake & Cylindrical Seal",
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
                SiteId = sinauli.Id,
                Name = "Royal Solid-Wheeled Bronze Age War Chariot",
                ArtefactType = "Full-Scale Two-Wheeled War Vehicle",
                Material = "Wood, Copper Plate Inlays, Bronze Hardware",
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
                SiteId = ur.Id,
                Name = "Standard of Ur",
                ArtefactType = "Hollow Wooden Box Mosaic",
                Material = "Lapis Lazuli, Red Limestone, and Shell set in Bitumen",
                ApproximateYear = -2600,
                Dimensions = "Length 49.5 cm, Height 21.5 cm",
                Description = "Dual-sided narrative mosaic box depicting 'War' and 'Peace'. Shows chariot warfare, prisoners, feast banquets, and trade tribute.",
                CurrentLocation = "The British Museum, London",
                DiscoveryContext = "Found in the Royal Cemetery of Ur, tomb PG 779",
                ImageUrl = "/images/artefacts/standard-of-ur.jpg",
                Model3DType = "cuneiform_tablet"
            },
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
                SiteId = angkorWat.Id,
                Name = "Khmer Bronze Avalokiteshvara with Silver Eyes",
                ArtefactType = "Lost-Wax Cast Bronze Icon",
                Material = "Bronze with Inlaid Silver Eyes",
                ApproximateYear = 1150,
                Dimensions = "Height 42 cm, Width 18 cm",
                Description = "Graceful eight-armed deity embodying compassion, wearing an intricate cylindrical sampot and jatamukuta headdress bearing Amitabha Buddha.",
                CurrentLocation = "National Museum of Cambodia, Phnom Penh",
                DiscoveryContext = "Discovered during gallery clearance near the Northern Library",
                ImageUrl = "/images/artefacts/angkor-wat-avalokiteshvara.jpg",
                Model3DType = "dancing_girl_bronze"
            },
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
            }
        );

        // ==========================================
        // 8. EXCAVATIONS & STRATIGRAPHY
        // ==========================================
        var excDholavira = new Excavation
        {
            SiteId = dholavira.Id,
            ExpeditionName = "Dholavira Systematic Scientific Project",
            LeadArchaeologist = "Dr. Ravindra Singh Bisht",
            StartYear = 1990,
            EndYear = 2005,
            Organization = "Archaeological Survey of India (Excavation Branch V)",
            Summary = "Fifteen seasons of excavations revealing 7 continuous cultural stages from pre-Harappan formative settlement through mature planning to post-urban decline."
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

        context.Excavations.AddRange(excDholavira, excKeeladi);
        await context.SaveChangesAsync();

        var layerD1 = new ExcavationLayer
        {
            ExcavationId = excDholavira.Id,
            LayerNumber = 1,
            LayerName = "Stage VII: Late Post-Urban Encampment",
            DepthMeters = 0.6,
            SoilComposition = "Loose wind-blown sand, aeolian deposit and crumbling rubble",
            EstimatedStartYear = -1650,
            EstimatedEndYear = -1500,
            CulturalAffiliation = "Late Harappan (Jhukar-like ceramic affinity)",
            Description = "Impoverished sub-urban circular stone hut structures without urban drainage or writing."
        };

        var layerD2 = new ExcavationLayer
        {
            ExcavationId = excDholavira.Id,
            LayerNumber = 2,
            LayerName = "Stage IV-V: Peak Mature Harappan Urban Horizon",
            DepthMeters = 2.4,
            SoilComposition = "Dense compacted occupational floor with burnt lime plaster and paved brick",
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
            SoilComposition = "Virgin weathered bedrock overlain with sterile riverine silt and non-Harappan red slip pottery",
            EstimatedStartYear = -3000,
            EstimatedEndYear = -2600,
            CulturalAffiliation = "Early Pre-Harappan",
            Description = "First stone and mudbrick fortification walls built directly over bed-rock; wheel-made bichrome and monochrome pottery."
        };

        var layerK1 = new ExcavationLayer
        {
            ExcavationId = excKeeladi.Id,
            LayerNumber = 1,
            LayerName = "Stratum IV: Early Historic Sangam Horizon (AMS 580 BCE)",
            DepthMeters = 2.8,
            SoilComposition = "Compacted dark alluvial clay with burnt brick fragments, charcoal nodules, and pot sherds",
            EstimatedStartYear = -600,
            EstimatedEndYear = -300,
            CulturalAffiliation = "Early Sangam (Old Tamil / Tamil-Brahmi)",
            Description = "Continuous occupational stratum yielding Tamil-Brahmi inscribed Black-and-Red ware potsherds, ring wells, and lapidary carnelian bead debitage."
        };

        context.ExcavationLayers.AddRange(layerD1, layerD2, layerD3, layerK1);
        await context.SaveChangesAsync();

        context.Findings.AddRange(
            new Finding
            {
                ExcavationLayerId = layerD2.Id,
                Name = "Unicorn Stamp Seal with Indus Script",
                FindingType = "Intaglio Seal",
                Description = "Squared steatite seal depicting a one-horned bovine and five sacred pictographs",
                YearFound = 1997
            },
            new Finding
            {
                ExcavationLayerId = layerK1.Id,
                Name = "Agate Micro-Drill Bead Core",
                FindingType = "Lapidary Tooling",
                Description = "High precision quartz drill-bits used for piercing semi-precious stone beads",
                YearFound = 2019
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
