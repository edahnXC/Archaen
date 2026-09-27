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
        // 1. CIVILIZATIONS
        // ==========================================
        var harappan = new Civilization
        {
            Name = "Indus Valley (Harappan) Civilization",
            Slug = "indus-valley-harappan",
            Region = "South Asia (Indus Basin & Gujarat)",
            StartYear = -3300,
            EndYear = -1300,
            ColorHex = "#E06A3B",
            PrimaryLanguage = "Undeciphered Indus Script",
            ArchitecturalTradition = "Standardized fired mudbrick, orthogonal grid planning, monumental stepwells and subterranean covered drains",
            Description = "One of the three earliest cradles of Old World civilization, distinguished by remarkable urban planning, standardized weights, maritime trading networks, and advanced hydraulic engineering without evidence of monarchical despotism."
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

        var mycenaean = new Civilization
        {
            Name = "Mycenaean Civilization",
            Slug = "mycenaean",
            Region = "Mainland Greece & Aegean",
            StartYear = -1750,
            EndYear = -1050,
            ColorHex = "#457B9D",
            PrimaryLanguage = "Mycenaean Greek (Linear B)",
            ArchitecturalTradition = "Cyclopean stone fortifications, megaron central halls, and monumental corbelled tholos tombs",
            Description = "Late Bronze Age mainland Greek civilization, immortalized in Homeric epic tradition, featuring fortified acropolises, warrior aristocracies, and tholos beehive tombs."
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

        var mesoamerican = new Civilization
        {
            Name = "Mesoamerican Classic Horizon",
            Slug = "mesoamerican-classic",
            Region = "Mesoamerica (Central Mexico)",
            StartYear = -200,
            EndYear = 750,
            ColorHex = "#588157",
            PrimaryLanguage = "Unrecorded (Pre-Nahuatl / Otomanguean)",
            ArchitecturalTradition = "Talud-tablero stepped pyramids, broad ceremonial avenues, and stuccoed painted residential compounds",
            Description = "Major multi-ethnic urban civilization of the Mexican highlands, marked by monumental volcanic stone pyramids, obsidian tool production, and astronomical grid alignment."
        };

        context.Civilizations.AddRange(harappan, mesopotamian, ancientEgyptian, minoan, mycenaean, roman, mesoamerican);
        await context.SaveChangesAsync();

        // ==========================================
        // 2. HISTORICAL PERIODS
        // ==========================================
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

        var middleBronze = new HistoricalPeriod
        {
            Name = "Middle Bronze Age / Palatial Era",
            Slug = "middle-bronze-age",
            Epoch = "Bronze Age",
            StartYear = -2100,
            EndYear = -1550,
            Description = "Flourishing of Minoan palaces on Crete, Middle Kingdom Egypt, and the Old Babylonian Empire."
        };

        var lateBronze = new HistoricalPeriod
        {
            Name = "Late Bronze Age International Era",
            Slug = "late-bronze-age",
            Epoch = "Bronze Age",
            StartYear = -1550,
            EndYear = -1177,
            Description = "Interconnected diplomatic and commercial sphere between Egypt, the Hittites, Mycenae, and Babylonia, ending in the Bronze Age Collapse."
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

        context.HistoricalPeriods.AddRange(earlyBronze, matureHarappanPeriod, middleBronze, lateBronze, classicalAntiquity);
        await context.SaveChangesAsync();

        // ==========================================
        // 3. ACADEMIC REFERENCES
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

        var refMarshall = new Reference
        {
            CitationKey = "Marshall1931",
            Authors = "Marshall, Sir John",
            PublicationYear = 1931,
            Title = "Mohenjo-daro and the Indus Civilization: Being an Official Account of Archaeological Excavations Carried Out by the Government of India between the Years 1922 and 1927",
            JournalOrPublisher = "Arthur Probsthain (London)"
        };

        var refKenoyer = new Reference
        {
            CitationKey = "Kenoyer1998",
            Authors = "Kenoyer, Jonathan Mark",
            PublicationYear = 1998,
            Title = "Ancient Cities of the Indus Valley Civilization",
            JournalOrPublisher = "Oxford University Press & American Institute of Pakistan Studies",
            DoiOrIsbn = "978-0195779400"
        };

        var refWoolley = new Reference
        {
            CitationKey = "Woolley1934",
            Authors = "Woolley, C. Leonard",
            PublicationYear = 1934,
            Title = "Ur Excavations, Volume II: The Royal Cemetery",
            JournalOrPublisher = "Trustees of the British Museum and the University of Pennsylvania Museum",
        };

        var refEvans = new Reference
        {
            CitationKey = "Evans1921",
            Authors = "Evans, Sir Arthur",
            PublicationYear = 1921,
            Title = "The Palace of Minos: A Comparative Account of the Successive Stages of the Early Cretan Civilization as Illustrated by the Discoveries at Knossos",
            JournalOrPublisher = "Macmillan and Co. (London)"
        };

        var refBeard = new Reference
        {
            CitationKey = "Beard2008",
            Authors = "Beard, Mary",
            PublicationYear = 2008,
            Title = "Pompeii: The Life of a Roman Town",
            JournalOrPublisher = "Profile Books (London)",
            DoiOrIsbn = "978-1861975966"
        };

        var refLehner = new Reference
        {
            CitationKey = "Lehner1997",
            Authors = "Lehner, Mark",
            PublicationYear = 1997,
            Title = "The Complete Pyramids: Solving the Ancient Mysteries",
            JournalOrPublisher = "Thames & Hudson (London)",
            DoiOrIsbn = "978-0500050842"
        };

        context.References.AddRange(refBisht, refRao, refMarshall, refKenoyer, refWoolley, refEvans, refBeard, refLehner);
        await context.SaveChangesAsync();

        // ==========================================
        // 4. ARCHAEOLOGICAL SITES
        // ==========================================

        // Site 1: Dholavira
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
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/Dholavira_Reservoir.jpg/1280px-Dholavira_Reservoir.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 2: Lothal
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
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/53/Lothal_dockyard.jpg/1280px-Lothal_dockyard.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 3: Mohenjo-daro
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
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c2/Mohenjo-daro.jpg/1280px-Mohenjo-daro.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 4: Harappa
        var harappa = new Site
        {
            Name = "Harappa",
            AncientName = "Hari-Yupiya (Hypothetical)",
            Slug = "harappa",
            Description = "The type-site of the Indus civilization situated on an ancient paleo-channel of the Ravi River. Featuring massive fortified Citadel Mound AB, the Circular Working Platforms for grain processing, the Great Granary, and extensive cemetery areas (R-37 and Cemetery H) documenting long-term chronological evolution from 3300 BCE to 1300 BCE.",
            Region = "Sahiwal District, Punjab",
            Country = "Pakistan",
            SiteType = "Metropolis & Fortified Urban Center",
            Latitude = 30.630000,
            Longitude = 72.866944,
            StartYear = -3300,
            EndYear = -1300,
            DatingPrecision = "Radiocarbon (14C) stratigraphic calibration by HARP",
            DiscoveryInformation = "Visited by Charles Masson in 1826, surveyed by Alexander Cunningham; excavated systematically by Daya Ram Sahni, M.S. Vats, and the Harappa Archaeological Research Project (HARP).",
            ExcavationStatus = "Excavated & Active Conservation",
            WaterSource = "Ancient bed of the River Ravi",
            ArchitecturalHighlights = "Citadel Mound AB ramparts, Circular Brick Platforms, Great Granary / Warehouse complex, Artisan Quarters",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5f/Harappa_excavated_walls.jpg/1280px-Harappa_excavated_walls.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 5: Ur (Tell el-Mukayyar)
        var ur = new Site
        {
            Name = "Ur (Tell el-Mukayyar)",
            AncientName = "Urim",
            Slug = "ur-tell-el-mukayyar",
            Description = "Major Sumerian coastal and riverine city-state near the mouth of the Euphrates. Renowned for the colossal mudbrick Ziggurat of Ur built by King Ur-Nammu, the Royal Cemetery containing gold and lapis lazuli grave goods of Queen Puabi, and extensive legal, administrative, and economic cuneiform archives.",
            Region = "Dhi Qar Governorate",
            Country = "Iraq",
            SiteType = "Sumerian City-State & Religious Ziggurat Complex",
            Latitude = 30.962222,
            Longitude = 46.104444,
            StartYear = -3800,
            EndYear = -500,
            DatingPrecision = "Cuneiform Epigraphy, King Lists, and Radiocarbon",
            DiscoveryInformation = "Excavated by J.E. Taylor (1853-54) and celebrated 1922-1934 joint British Museum / Penn Museum expeditions directed by Sir Leonard Woolley.",
            ExcavationStatus = "Excavated; UNESCO World Heritage Site (2016)",
            WaterSource = "Euphrates River historical course and maritime canals to the Persian Gulf",
            ArchitecturalHighlights = "Ziggurat of Ur-Nammu, Royal Tombs, Gipar-ku temple, baked brick vaults and corbelled burial chambers",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1d/Ziggurat_of_ur.jpg/1280px-Ziggurat_of_ur.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 6: Giza Necropolis
        var giza = new Site
        {
            Name = "Giza Necropolis",
            AncientName = "Imentet (The West)",
            Slug = "giza-necropolis",
            Description = "The iconic Old Kingdom pyramid complex on the Giza plateau. Comprises the Great Pyramid of Khufu, the Pyramid of Khafre with the Great Sphinx, the Pyramid of Menkaure, subsidiary pyramids, mastaba cemeteries of royal courtiers, and the excavated Lost City of the Pyramid Builders (Heit el-Ghurab).",
            Region = "Giza Governorate",
            Country = "Egypt",
            SiteType = "Pyramid Necropolis & Royal Mortuary Complex",
            Latitude = 29.979167,
            Longitude = 31.134167,
            StartYear = -2580,
            EndYear = -2150,
            DatingPrecision = "Astronomical alignments, Quarry worker graffiti & Radiocarbon",
            DiscoveryInformation = "Documented by Flinders Petrie, George Reisner; modern excavations of workers' settlements by Mark Lehner (AERA) and Zahi Hawass.",
            ExcavationStatus = "Excavated; UNESCO World Heritage Site (1979)",
            WaterSource = "Nile River inundation canal basins and harbour quays",
            ArchitecturalHighlights = "Great Pyramid ashlar limestone masonry, granite King's Chamber, Valley Temples, Great Sphinx carved from limestone bedrock",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e3/Kheops-Pyramid.jpg/1280px-Kheops-Pyramid.jpg",
            IsUnescoWorldHeritage = true
        };

        // Site 7: Knossos
        var knossos = new Site
        {
            Name = "Knossos",
            AncientName = "Ko-no-so (Linear B)",
            Slug = "knossos",
            Description = "The monumental central administrative, ceremonial, and religious palace complex of Bronze Age Minoan Crete. Organised around an expansive rectangular Central Court, it displays multi-tiered ashlar architecture, monumental gypsum pillar crypts, lightwells, terracotta drainage pipes, and vivid polychrome frescoes.",
            Region = "Heraklion, Crete",
            Country = "Greece",
            SiteType = "Palatial Administrative Center & Sanctuary",
            Latitude = 35.297778,
            Longitude = 25.163056,
            StartYear = -2000,
            EndYear = -1380,
            DatingPrecision = "Stratigraphic Ceramic Sequence (EM, MM, LM) & Cross-Dating with Egyptian Dynasties",
            DiscoveryInformation = "Discovered by Minos Kalokairinos (1878); purchased and excavated systematically by Sir Arthur Evans from 1900 onwards.",
            ExcavationStatus = "Excavated & Reconstituted",
            WaterSource = "Kairatos River valley spring channels and sophisticated terracotta pressure aqueducts",
            ArchitecturalHighlights = "Central Court, Throne Room with alabaster throne, Grand Staircase, West Magazines holding monumental pithoi jars, lightwells",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b7/Palace_of_Knossos.jpg/1280px-Palace_of_Knossos.jpg",
            IsUnescoWorldHeritage = false
        };

        // Site 8: Pompeii
        var pompeii = new Site
        {
            Name = "Pompeii",
            AncientName = "Colonia Cornelia Veneria Pompeianorum",
            Slug = "pompeii",
            Description = "An impeccably preserved Roman urban town in Campania, buried beneath 4 to 6 meters of volcanic ash and pumice during the eruption of Mount Vesuvius in 79 CE. Offers an unmatched window into Roman municipal governance, atrium domus architecture, thermopolia (food shops), baths, amphitheaters, and wall paintings.",
            Region = "Campania (Metropolitan Naples)",
            Country = "Italy",
            SiteType = "Preserved Roman City & Port",
            Latitude = 40.750833,
            Longitude = 14.486944,
            StartYear = -600,
            EndYear = 79,
            DatingPrecision = "Historical accounts (Pliny the Younger) & Volcanic tephrochronology (79 CE terminus)",
            DiscoveryInformation = "Rediscovered in 1599 by Domenico Fontana; excavations commenced in 1748 under Roque Joaquín de Alcubierre for King Charles VII of Naples.",
            ExcavationStatus = "Extensively Excavated; UNESCO World Heritage Site (1997)",
            WaterSource = "Aqua Augusta (Serino aqueduct) feeding castellum aquae water towers and lead pressure pipes",
            ArchitecturalHighlights = "Forum Civic Complex, Villa of the Mysteries, House of the Faun, Amphitheatre, Stabian Baths, paved basalt streets with stepping stones",
            ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/Pompeii_Forum_facing_Vesuvius.jpg/1280px-Pompeii_Forum_facing_Vesuvius.jpg",
            IsUnescoWorldHeritage = true
        };

        context.Sites.AddRange(dholavira, lothal, mohenjodaro, harappa, ur, giza, knossos, pompeii);
        await context.SaveChangesAsync();

        // ==========================================
        // 5. SITE-CIVILIZATION & SITE-PERIOD JOINS
        // ==========================================
        context.SiteCivilizations.AddRange(
            new SiteCivilization { SiteId = dholavira.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = lothal.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = mohenjodaro.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = harappa.Id, CivilizationId = harappan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = ur.Id, CivilizationId = mesopotamian.Id, IsPrimary = true },
            new SiteCivilization { SiteId = giza.Id, CivilizationId = ancientEgyptian.Id, IsPrimary = true },
            new SiteCivilization { SiteId = knossos.Id, CivilizationId = minoan.Id, IsPrimary = true },
            new SiteCivilization { SiteId = pompeii.Id, CivilizationId = roman.Id, IsPrimary = true }
        );

        context.SitePeriods.AddRange(
            new SitePeriod { SiteId = dholavira.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = lothal.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = mohenjodaro.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = harappa.Id, HistoricalPeriodId = earlyBronze.Id },
            new SitePeriod { SiteId = harappa.Id, HistoricalPeriodId = matureHarappanPeriod.Id },
            new SitePeriod { SiteId = ur.Id, HistoricalPeriodId = earlyBronze.Id },
            new SitePeriod { SiteId = giza.Id, HistoricalPeriodId = earlyBronze.Id },
            new SitePeriod { SiteId = knossos.Id, HistoricalPeriodId = middleBronze.Id },
            new SitePeriod { SiteId = pompeii.Id, HistoricalPeriodId = classicalAntiquity.Id }
        );

        // ==========================================
        // 6. SITE CITATIONS
        // ==========================================
        context.SiteReferences.AddRange(
            new SiteReference { SiteId = dholavira.Id, ReferenceId = refBisht.Id, SpecificPagesOrPlates = "pp. 45-120; Plates XII-XXX" },
            new SiteReference { SiteId = lothal.Id, ReferenceId = refRao.Id, SpecificPagesOrPlates = "Vol I, pp. 23-88; Dockyard hydraulic analysis" },
            new SiteReference { SiteId = mohenjodaro.Id, ReferenceId = refMarshall.Id, SpecificPagesOrPlates = "Vol I, Chapter 3: The Great Bath" },
            new SiteReference { SiteId = harappa.Id, ReferenceId = refKenoyer.Id, SpecificPagesOrPlates = "pp. 55-92; Mound AB and Cemetery R-37" },
            new SiteReference { SiteId = ur.Id, ReferenceId = refWoolley.Id, SpecificPagesOrPlates = "pp. 12-45; Royal Tombs PG 789 and PG 1237" },
            new SiteReference { SiteId = giza.Id, ReferenceId = refLehner.Id, SpecificPagesOrPlates = "pp. 108-133; The Great Pyramid of Khufu" },
            new SiteReference { SiteId = knossos.Id, ReferenceId = refEvans.Id, SpecificPagesOrPlates = "Vol I, pp. 200-245; The Central Court" },
            new SiteReference { SiteId = pompeii.Id, ReferenceId = refBeard.Id, SpecificPagesOrPlates = "pp. 75-102; Street life and domestic housing" }
        );

        // ==========================================
        // 7. DIAGNOSTIC ARTEFACTS (Real verified artefacts with 3D model identifiers)
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
                Description = "A set of ten monumental Indus symbols discovered in a room adjoining the Western Gateway of the Citadel. Each symbol was crafted from crystalline gypsum and originally affixed to a wooden facade.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Discovered fallen face-down inside the Western Gateway of the Dholavira Citadel (Stratigraphic Stage IV)",
                Model3DType = "stone_stele"
            },
            new Artefact
            {
                SiteId = lothal.Id,
                Name = "Persian Gulf Steatite Button Seal",
                ArtefactType = "Circular Compartmented Stamp Seal",
                Material = "Glazed Steatite",
                ApproximateYear = -2100,
                Dimensions = "Diameter 2.25 cm, Thickness 0.6 cm",
                Description = "A circular steatite seal with two jumping ibexes flanking a sun motif, typical of Dilmun (Bahrain) and the Persian Gulf, providing unequivocal physical evidence of long-distance direct maritime trade between Lothal and Mesopotamia.",
                CurrentLocation = "Archaeological Museum, Lothal",
                DiscoveryContext = "Found in the Warehouse area near the Tidal Dockyard basin (Lothal Phase III)",
                Model3DType = "seal_cube"
            },
            new Artefact
            {
                SiteId = mohenjodaro.Id,
                Name = "The Dancing Girl of Mohenjo-daro",
                ArtefactType = "Cast Bronze Statuette",
                Material = "Bronze (Lost-Wax / Cire Perdue Casting)",
                ApproximateYear = -2300,
                Dimensions = "Height 10.5 cm, Width 5 cm",
                Description = "World-famous masterpiece of Bronze Age naturalism, depicting a young girl standing in a confident posture with right hand on hip, left arm adorned with 24 bangles, and hair tied in a voluminous side chignon.",
                CurrentLocation = "National Museum, New Delhi",
                DiscoveryContext = "Found in 1926 by Ernest Mackay in the HR Area of Mohenjo-daro",
                Model3DType = "dancing_girl_bronze"
            },
            new Artefact
            {
                SiteId = mohenjodaro.Id,
                Name = "The Priest-King",
                ArtefactType = "Sculpture in the Round",
                Material = "Low-fired Steatite with traces of red pigment",
                ApproximateYear = -2200,
                Dimensions = "Height 17.5 cm, Width 11 cm",
                Description = "Bearded male figure wearing a fillet headband with a central circular jewel and an off-the-shoulder cloak embroidered with trefoil motifs originally filled with red paste.",
                CurrentLocation = "National Museum of Pakistan, Karachi",
                DiscoveryContext = "Found in the DK-B area, depth 1.37 meters below surface",
                Model3DType = "stone_stele"
            },
            new Artefact
            {
                SiteId = ur.Id,
                Name = "Standard of Ur",
                ArtefactType = "Hollow Wooden Box Mosaic",
                Material = "Lapis Lazuli, Red Limestone, and Shell set in Bitumen",
                ApproximateYear = -2600,
                Dimensions = "Length 49.5 cm, Height 21.5 cm",
                Description = "Dual-sided narrative mosaic box depicting 'War' (Sumerian chariots, spearmen, prisoners) and 'Peace' (royal banquet with lyre player and tribute bearers).",
                CurrentLocation = "The British Museum, London",
                DiscoveryContext = "Found in the Royal Cemetery of Ur, tomb PG 779",
                Model3DType = "cuneiform_tablet"
            },
            new Artefact
            {
                SiteId = knossos.Id,
                Name = "The Snake Goddess Figurine",
                ArtefactType = "Faience Figurine",
                Material = "Polychrome Glazed Faience",
                ApproximateYear = -1600,
                Dimensions = "Height 29.5 cm",
                Description = "Figurine of a woman dressed in a tiered flounced skirt and tight bodice exposing her breasts, holding writhing snakes in both hands with a feline perched atop her headdress.",
                CurrentLocation = "Heraklion Archaeological Museum, Crete",
                DiscoveryContext = "Found by Arthur Evans in the Temple Repositories of the Palace of Knossos",
                Model3DType = "pottery_amphora"
            }
        );

        // ==========================================
        // 8. EXCAVATIONS & STRATIGRAPHY (Phase 10)
        // ==========================================
        var excDholavira = new Excavation
        {
            SiteId = dholavira.Id,
            ExpeditionName = "Dholavira Systematic Scientific Project",
            LeadArchaeologist = "Dr. Ravindra Singh Bisht",
            StartYear = 1990,
            EndYear = 2005,
            Organization = "Archaeological Survey of India (Excavation Branch V)",
            Summary = "Fifteen seasons of extensive excavations revealing 7 continuous cultural stages from pre-Harappan formative settlement through mature planning to post-urban decline."
        };

        context.Excavations.Add(excDholavira);
        await context.SaveChangesAsync();

        var layer1 = new ExcavationLayer
        {
            ExcavationId = excDholavira.Id,
            LayerNumber = 1,
            LayerName = "Stage VII: Late Post-Urban Encampment",
            DepthMeters = 0.6,
            SoilComposition = "Loose wind-blown sand, aeolian deposit and crumbling rubble",
            EstimatedStartYear = -1650,
            EstimatedEndYear = -1500,
            CulturalAffiliation = "Late Harappan (Jhukar-like ceramic affinity)",
            Description = "Impoverished sub-urban circular stone hut structures without urban drainage or writing; marked reduction in site size."
        };

        var layer2 = new ExcavationLayer
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

        var layer3 = new ExcavationLayer
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

        context.ExcavationLayers.AddRange(layer1, layer2, layer3);
        await context.SaveChangesAsync();

        context.Findings.AddRange(
            new Finding
            {
                ExcavationLayerId = layer1.Id,
                Name = "Coarse Ocher Pot Shards",
                FindingType = "Diagnostic Pottery",
                Description = "Degenerate wheel-turned pots with simple incised bands",
                YearFound = 1993
            },
            new Finding
            {
                ExcavationLayerId = layer2.Id,
                Name = "Unicorn Stamp Seal with Indus Script",
                FindingType = "Intaglio Seal",
                Description = "Squared steatite seal depicting a one-horned bovine and five sacred pictographs",
                YearFound = 1997
            },
            new Finding
            {
                ExcavationLayerId = layer3.Id,
                Name = "Chert Micro-blades",
                FindingType = "Lithic Tools",
                Description = "High precision knapped blades made from Rohri chert cores",
                YearFound = 1999
            }
        );

        // ==========================================
        // 9. ARCHAEOLOGICAL RELATIONSHIPS (Phase 9)
        // ==========================================
        context.SiteRelationships.AddRange(
            new SiteRelationship
            {
                SourceSiteId = lothal.Id,
                TargetSiteId = dholavira.Id,
                RelationshipType = "Maritime & Inland Trade Corridor",
                Description = "Lothal operated as the coastal port and carnelian lapidary manufacturing depot supplying processed luxury beads to Dholavira's transit gateway heading to the Makran coast and Mesopotamia."
            },
            new SiteRelationship
            {
                SourceSiteId = dholavira.Id,
                TargetSiteId = mohenjodaro.Id,
                RelationshipType = "Contemporaneous Metropolis",
                Description = "Both metropolises flourished contemporaneously during the Integration Era (-2500 to -1900 BCE), utilizing identical 16-ratio standardized weights, script symbols, and fired brick proportions (1:2:4)."
            },
            new SiteRelationship
            {
                SourceSiteId = harappa.Id,
                TargetSiteId = mohenjodaro.Id,
                RelationshipType = "Sister Capital Axis",
                Description = "The northern and southern twin hubs of the Indus drainage network, linked by seasonal riverine traffic across the Indus and Ravi waterways."
            },
            new SiteRelationship
            {
                SourceSiteId = lothal.Id,
                TargetSiteId = ur.Id,
                RelationshipType = "Direct Long-Distance Maritime Commerce (Meluhha-Mesopotamia)",
                Description = "Documented by Persian Gulf seals and cuneiform trade records referencing seafaring merchants of Meluhha bringing carnelian beads, copper ingots, and exotic timbers into the port of Ur."
            }
        );

        await context.SaveChangesAsync();
    }
}
