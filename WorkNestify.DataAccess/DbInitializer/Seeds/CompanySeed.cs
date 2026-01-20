using WorkNestify.Models.Models.Companies;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.DataAccess.DbInitializer.Seeds;

public static class CompanySeed
{
    public static List<Company> GetCompanies()
    {
        return new List<Company>
        {
            new Company
            {
                Name = "FPT Corporation",
                Website = "https://fpt.com/en",
                Email = "ir@fpt.com",
                Phone = "+84 24 7300 7300",
                StreetAddress = "FPT Tower, 10 Pham Van Bach Street",
                Description =
                    "<h1>About FPT Corporation</h1>  \n\n<h2>Overview</h2>  \n<p>FPT Corporation is a leading technology and IT services company based in Vietnam. Established in 1988, it has grown into one of the most influential technology firms in the country.</p>  \n\n<h2>Core Services</h2>  \n<p>FPT specializes in various technology sectors, including software development, digital transformation, telecommunications, and education. The company provides innovative solutions to businesses worldwide, helping them enhance efficiency and competitiveness.</p>  \n\n<h2>Industry Presence</h2>  \n<p>With a strong presence in industries such as finance, healthcare, manufacturing, and retail, FPT continuously supports enterprises in modernizing their operations through advanced digital solutions.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>FPT invests heavily in cutting-edge technologies like artificial intelligence (AI), cloud computing, big data, and the Internet of Things (IoT) to drive digital transformation and industry disruption.</p>  \n\n<h2>Global Expansion</h2>  \n<p>Operating in over 25 countries, FPT is committed to expanding its global footprint. By partnering with major international firms, it delivers high-quality technology services to businesses worldwide.</p>  \n\n<h2>Commitment to the Future</h2>  \n<p>FPT remains dedicated to innovation, talent development, and technological advancement, shaping the future of digital transformation and contributing to the global tech ecosystem.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/1/11/FPT_logo_2010.svg",
                Industry = "Information technology, telecommunications, education",
                FoundedDate = new DateTime(1988, 09, 13),
                Size = CompanySizes.Large,
                ProvinceCode = 1,
                WardCode = 166
            },
            new Company
            {
                Name = "Base.vn",
                Website = "https://base.vn",
                Email = "contact@base.vn",
                Phone = "+84 24 2244 1313",
                StreetAddress = "123 Nguyen Trai Street",
                Description =
                    "<h1>About Base.vn</h1>  \n\n<h2>Overview</h2>  \n<p>Base.vn is a leading SaaS company in Vietnam, offering digital transformation solutions to help enterprises optimize their operations and improve efficiency.</p>  \n\n<h2>Core Services</h2>  \n<p>Base.vn provides a wide range of cloud-based software solutions, including workflow automation, human resource management, business intelligence, and collaboration tools.</p>  \n\n<h2>Industry Presence</h2>  \n<p>Trusted by thousands of businesses across various industries, Base.vn empowers organizations to streamline processes, enhance productivity, and drive digital innovation.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>With a strong focus on technology, Base.vn leverages AI, big data, and automation to develop intelligent enterprise solutions that cater to modern business needs.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Base.vn continues to expand its ecosystem of products and services, supporting businesses in their digital transformation journey across Vietnam and beyond.</p>  \n\n<h2>Commitment to Businesses</h2>  \n<p>Dedicated to helping companies succeed in the digital age, Base.vn remains committed to innovation, customer success, and delivering cutting-edge SaaS solutions.</p>  \n",
                Logo = "https://static-gcdn.basecdn.net/landing/base.vn/image/v2/logo/base.png",
                Industry = "Software as a Service (SaaS)",
                FoundedDate = new DateTime(2016, 8, 1),
                Size = CompanySizes.Small,
                ProvinceCode = 1,
                WardCode = 367
            },
            new Company
            {
                Name = "KiotViet",
                Website = "https://www.kiotviet.vn",
                Email = "hotro@kiotviet.vn",
                Phone = "+84 86 2533 433",
                StreetAddress = "C Section, Waseco Building, 10 Pho Quang Street",
                Description =
                    "<h1>About KiotViet</h1>  \n\n<h2>Overview</h2>  \n<p>KiotViet provides cloud-based POS and business management software tailored for small businesses, helping them streamline operations and improve efficiency.</p>  \n\n<h2>Core Services</h2>  \n<p>KiotViet offers a comprehensive suite of solutions, including point-of-sale (POS) systems, inventory management, customer relationship management (CRM), and sales tracking.</p>  \n\n<h2>Industry Presence</h2>  \n<p>Serving over 150,000 businesses across various sectors, KiotViet is a trusted solution for retailers, wholesalers, and service providers looking to digitize their operations.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>KiotViet leverages cloud computing, data analytics, and automation to provide scalable and user-friendly business management tools that enhance decision-making.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>With a rapidly growing user base, KiotViet continues to expand its features and services, ensuring businesses have access to the latest digital solutions for growth.</p>  \n\n<h2>Commitment to Small Businesses</h2>  \n<p>KiotViet is dedicated to empowering small and medium-sized enterprises (SMEs) with affordable, efficient, and easy-to-use software, supporting their success in the digital economy.</p>  \n",
                Logo = "https://logo.kiotviet.vn/KiotViet-Logo-Horizontal.svg",
                Industry = "Retail Technology, POS Systems",
                FoundedDate = new DateTime(2014, 5, 15),
                Size = CompanySizes.Small,
                ProvinceCode = 2,
                WardCode = 26977
            },
            new Company
            {
                Name = "The Coffee House",
                Website = "https://www.thecoffeehouse.com",
                Email = "hi@thecoffeehouse.com",
                Phone = "+84 28 7107 8079",
                StreetAddress = "86 - 88 Cao Thang Street",
                Description =
                    "<h1>About The Coffee House</h1>  \n\n<h2>Overview</h2>  \n<p>The Coffee House is a fast-growing Vietnamese coffee chain known for its modern, cozy atmosphere and high-quality beverages.</p>  \n\n<h2>Signature Offerings</h2>  \n<p>The Coffee House serves a diverse menu of specialty coffee, tea, and pastries, crafted to suit the tastes of Vietnamese coffee lovers.</p>  \n\n<h2>Store Experience</h2>  \n<p>Designed as a welcoming space for work, study, and social gatherings, each store provides a comfortable ambiance with contemporary decor and friendly service.</p>  \n\n<h2>Commitment to Quality</h2>  \n<p>With a focus on sustainability, The Coffee House sources high-quality coffee beans from local farmers, ensuring fresh and flavorful drinks in every cup.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Since its launch, The Coffee House has rapidly expanded across Vietnam, becoming a favorite destination for coffee enthusiasts and casual customers alike.</p>  \n\n<h2>Community Engagement</h2>  \n<p>Beyond serving coffee, The Coffee House fosters a strong community culture, encouraging creativity, connections, and shared experiences in every store.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/9/97/The_Coffee_House_logo.svg",
                Industry = "Food & Beverage, Coffee Retail",
                FoundedDate = new DateTime(2014, 7, 1),
                Size = CompanySizes.Medium,
                ProvinceCode = 2,
                WardCode = 27139
            },
            new Company
            {
                Name = "Foody.vn",
                Website = "https://www.foody.vn",
                Email = "support@shopeefood.vn",
                Phone = "+84 28 7300 2200",
                StreetAddress = "Jabes 1 Building, 244 Cong Quynh Street",
                Description =
                    "<h1>About Foody.vn</h1>  \n\n<h2>Overview</h2>  \n<p>Foody.vn is a leading food discovery, restaurant review, and delivery platform in Vietnam, connecting users with thousands of dining options across the country.</p>  \n\n<h2>Core Services</h2>  \n<p>Foody.vn offers restaurant reviews, user ratings, and a seamless food delivery service, helping customers find and order from their favorite eateries.</p>  \n\n<h2>Industry Presence</h2>  \n<p>With a vast database of restaurants, cafes, and street food vendors, Foody.vn serves as a go-to platform for food enthusiasts looking for new culinary experiences.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Foody.vn leverages data-driven recommendations, AI-powered search, and an intuitive mobile app to enhance the food discovery and ordering experience.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Since its launch, Foody.vn has expanded its services nationwide, partnering with restaurants and delivery providers to bring convenience to millions of users.</p>  \n\n<h2>Commitment to Users</h2>  \n<p>Foody.vn is dedicated to making dining more accessible and enjoyable by continuously improving its platform and providing reliable, user-generated reviews.</p>  \n",
                Logo = "https://www.foody.vn/style/images/logo/foody-vn.png",
                Industry = "Food Delivery, E-commerce",
                FoundedDate = new DateTime(2012, 6, 5),
                Size = CompanySizes.Medium,
                ProvinceCode = 2,
                WardCode = 26743
            },
            new Company
            {
                Name = "Digiworld",
                Website = "https://digiworld.com.vn",
                Email = "contact@digiworld.com.vn",
                Phone = "+84 28 3802 8700",
                StreetAddress = "14th Floor, TNR Tower, 54A Nguyen Chi Thanh Street",
                Description =
                    "<h1>About Digiworld</h1>  \n\n<h2>Overview</h2>  \n<p>Digiworld is a leading distributor of technology products, including laptops, smartphones, and accessories, serving as a key bridge between global brands and the Vietnamese market.</p>  \n\n<h2>Core Services</h2>  \n<p>Digiworld specializes in product distribution, market expansion, logistics, and after-sales services, ensuring seamless operations for its partners and customers.</p>  \n\n<h2>Industry Presence</h2>  \n<p>As a trusted distributor, Digiworld collaborates with top international tech brands, bringing high-quality products to consumers and businesses across Vietnam.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Leveraging advanced supply chain management and digital transformation, Digiworld optimizes distribution channels to enhance efficiency and market reach.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>With a strong nationwide network, Digiworld continues to expand its portfolio, covering diverse technology segments such as computing, mobility, and smart devices.</p>  \n\n<h2>Commitment to Excellence</h2>  \n<p>Digiworld is committed to delivering top-tier products and services, supporting brand growth, and enriching Vietnam’s technology landscape.</p>  \n",
                Logo = "https://digiworld.com.vn/assets/site/homes/logo/Artboard-1.png",
                Industry = "Technology, Distribution",
                FoundedDate = new DateTime(1997, 9, 1),
                Size = CompanySizes.Large,
                ProvinceCode = 1,
                WardCode = 199
            },
            new Company
            {
                Name = "Techcombank",
                Website = "https://www.techcombank.com.vn",
                Email = "contact@techcombank.com.vn",
                Phone = "+84 24 3944 6368",
                StreetAddress = "6 Quang Trung",
                Description =
                    "<h1>About Techcombank</h1>  \n\n<h2>Overview</h2>  \n<p>Techcombank is one of Vietnam’s leading commercial banks, providing a wide range of financial services to individuals, businesses, and institutions.</p>  \n\n<h2>Core Services</h2>  \n<p>Techcombank offers retail banking, corporate banking, wealth management, digital banking, and investment services, catering to diverse financial needs.</p>  \n\n<h2>Industry Presence</h2>  \n<p>With a strong presence across Vietnam, Techcombank serves millions of customers and is recognized for its innovative banking solutions and customer-centric approach.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Techcombank invests heavily in digital banking technologies, offering a seamless online and mobile banking experience with features like instant payments, digital loans, and AI-driven financial advice.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Techcombank continues to expand its network of branches and ATMs nationwide while enhancing its digital offerings to reach a broader customer base.</p>  \n\n<h2>Commitment to Excellence</h2>  \n<p>Committed to delivering exceptional financial services, Techcombank focuses on innovation, sustainability, and empowering customers to achieve their financial goals.</p>  \n",
                Logo = "https://dongphucvina.vn/wp-content/uploads/2023/05/logo-techcombank-dongphucvina.vn_.png",
                Industry = "Banking, Financial Services",
                FoundedDate = new DateTime(1993, 9, 27),
                Size = CompanySizes.Large,
                ProvinceCode = 1,
                WardCode = 190
            },
            new Company
            {
                Name = "VinFast",
                Website = "https://vinfastauto.com",
                Email = "info@vinfastauto.com",
                Phone = "+84 1900 232389",
                StreetAddress = "Vinhomes Riverside, Long Bien",
                Description =
                    "<h1>About VinFast</h1>  \n\n<h2>Overview</h2>  \n<p>VinFast is Vietnam’s first global automotive manufacturer, producing electric vehicles (EVs), scooters, and cars with a focus on sustainability and innovation.</p>  \n\n<h2>Core Products</h2>  \n<p>VinFast designs and manufactures a range of electric vehicles, including SUVs, sedans, and e-scooters, as well as offering smart mobility solutions.</p>  \n\n<h2>Industry Presence</h2>  \n<p>As a pioneer in Vietnam’s automotive industry, VinFast has made significant inroads into international markets, including North America and Europe.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>VinFast leverages cutting-edge technologies such as AI, battery management systems, and autonomous driving to create smart, eco-friendly vehicles.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>With ambitious plans for global expansion, VinFast is rapidly scaling its production capabilities and establishing a strong presence in the EV market.</p>  \n\n<h2>Commitment to Sustainability</h2>  \n<p>VinFast is dedicated to promoting sustainable transportation by accelerating the adoption of electric vehicles and reducing carbon emissions worldwide.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/a/ac/Logo_of_VinFast_%283D_Banner%29.svg",
                Industry = "Automotive, Electric Vehicles",
                FoundedDate = new DateTime(2017, 9, 2),
                Size = CompanySizes.Large,
                ProvinceCode = 1,
                WardCode = 136
            },
            new Company
            {
                Name = "Shopee Vietnam",
                Website = "https://shopee.vn",
                Email = "support@shopee.vn",
                Phone = "+84 1900 1221",
                StreetAddress = "Floor 4 - 5 - 6, Capital Palace Building, 29 Lieu Giai Street",
                Description =
                    "<h1>About Shopee Vietnam</h1>  \n\n<h2>Overview</h2>  \n<p>Shopee Vietnam is a leading e-commerce platform, offering a wide range of products and services to millions of users across the country.</p>  \n\n<h2>Core Services</h2>  \n<p>Shopee provides an online marketplace for buying and selling goods, logistics support, and digital payment solutions through ShopeePay.</p>  \n\n<h2>Industry Presence</h2>  \n<p>Shopee is a dominant player in Vietnam’s e-commerce sector, connecting sellers and buyers while offering promotions, flash sales, and a seamless shopping experience.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Shopee leverages AI, machine learning, and data analytics to personalize recommendations, optimize logistics, and enhance user engagement.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Since entering the Vietnamese market, Shopee has seen exponential growth, expanding its user base and product categories to meet diverse consumer needs.</p>  \n\n<h2>Commitment to Community</h2>  \n<p>Shopee is committed to empowering small businesses and entrepreneurs by providing them with a platform to reach a wider audience and grow their brands.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/f/fe/Shopee.svg",
                Industry = "E-commerce, Online Retail",
                FoundedDate = new DateTime(2016, 1, 1),
                Size = CompanySizes.Large,
                ProvinceCode = 1,
                WardCode = 8
            },
            new Company
            {
                Name = "VNG Corporation",
                Website = "https://www.vng.com.vn",
                Email = "info@vng.com.vn",
                Phone = "+84 28 3962 3888",
                StreetAddress = "Z06, 13 Street, Tan Thuan Dong",
                Description =
                    "<h1>About VNG Corporation</h1>  \n\n<h2>Overview</h2>  \n<p>VNG Corporation is a leading technology company in Vietnam, specializing in online gaming, digital content, and cloud services.</p>  \n\n<h2>Core Services</h2>  \n<p>VNG develops and publishes online games, provides cloud computing solutions, and offers digital payment services through ZaloPay.</p>  \n\n<h2>Industry Presence</h2>  \n<p>With popular products like Zalo and Zing MP3, VNG has a strong foothold in Vietnam’s tech and entertainment sectors, serving millions of users.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>VNG invests in AI, cloud infrastructure, and fintech to deliver innovative products and services that enhance digital experiences.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>VNG continues to expand its portfolio, exploring new markets and technologies while maintaining its position as a tech leader in Vietnam.</p>  \n\n<h2>Commitment to Innovation</h2>  \n<p>VNG is dedicated to fostering innovation, supporting local talent, and contributing to Vietnam’s digital economy through cutting-edge technology.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/8/8f/VNG_Corp._logo.svg",
                Industry = "Gaming, Technology, Fintech",
                FoundedDate = new DateTime(2004, 9, 9),
                Size = CompanySizes.Large,
                ProvinceCode = 2,
                WardCode = 27478
            },
            new Company
            {
                Name = "Sun Asterisk Vietnam",
                Website = "https://sun-asterisk.vn",
                Email = "contact@sun-asterisk.vn",
                Phone = "+84 236 388 9595",
                StreetAddress = "123 Le Loi Street",
                Description =
                    "<h1>About Sun Asterisk Vietnam</h1>  \n\n<h2>Overview</h2>  \n<p>Sun Asterisk Vietnam is a software development company that specializes in digital transformation, product development, and IT consulting.</p>  \n\n<h2>Core Services</h2>  \n<p>Sun Asterisk offers end-to-end software development, including web and mobile app development, UI/UX design, and cloud solutions.</p>  \n\n<h2>Industry Presence</h2>  \n<p>With a focus on startups and enterprises, Sun Asterisk partners with clients globally to deliver innovative digital products and services.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Sun Asterisk leverages modern frameworks, cloud technologies, and agile methodologies to build scalable and user-centric solutions.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Headquartered in Japan with a strong presence in Vietnam, Sun Asterisk continues to grow its operations, focusing on talent development and global outreach.</p>  \n\n<h2>Commitment to Quality</h2>  \n<p>Sun Asterisk is committed to delivering high-quality software solutions, fostering innovation, and supporting businesses in their digital journey.</p>  \n",
                Logo = "https://sun-asterisk.vn/wp-content/uploads/2020/10/logo-sun@2x.png",
                Industry = "Software Development, IT Consulting",
                FoundedDate = new DateTime(2012, 10, 1),
                Size = CompanySizes.Medium,
                ProvinceCode = 2,
                WardCode = 26740
            },
            new Company
            {
                Name = "Highlands Coffee",
                Website = "https://www.highlandscoffee.com.vn",
                Email = "support@highlandscoffee.com.vn",
                Phone = "+84 28 3829 8888",
                StreetAddress = "216 Tran Quang Khai",
                Description =
                    "<h1>About Highlands Coffee</h1>  \n\n<h2>Overview</h2>  \n<p>Highlands Coffee is Vietnam’s largest coffee chain, known for its authentic Vietnamese coffee and modern café experience.</p>  \n\n<h2>Signature Offerings</h2>  \n<p>Highlands Coffee serves traditional Vietnamese phin coffee, espresso-based drinks, tea, and snacks, emphasizing quality and authenticity.</p>  \n\n<h2>Store Experience</h2>  \n<p>With hundreds of locations nationwide, Highlands Coffee offers a cozy and contemporary space for customers to relax, work, or socialize.</p>  \n\n<h2>Commitment to Quality</h2>  \n<p>Highlands sources premium coffee beans from Vietnam’s Central Highlands, ensuring rich, authentic flavors in every cup.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Highlands Coffee has expanded rapidly across Vietnam and into international markets, becoming a cultural icon for Vietnamese coffee lovers.</p>  \n\n<h2>Community Engagement</h2>  \n<p>Highlands Coffee fosters community connections through its stores, promoting Vietnamese coffee culture and supporting local farmers.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/3/3d/Highlands_Coffee_5G.svg",
                Industry = "Food & Beverage, Coffee Retail",
                FoundedDate = new DateTime(1999, 1, 1),
                Size = CompanySizes.Large,
                ProvinceCode = 1,
                WardCode = 70
            },
            new Company
            {
                Name = "Viettel Group",
                Website = "https://viettel.com.vn",
                Email = "contact@viettel.com.vn",
                Phone = "+84 24 6255 6789",
                StreetAddress = "D26, Khu đô thị mới Cầu Giấy",
                Description =
                    "<h1>About Viettel Group</h1>  \n\n<h2>Overview</h2>  \n<p>Viettel Group is Vietnam’s largest telecommunications company, providing mobile, internet, and digital services to millions of customers.</p>  \n\n<h2>Core Services</h2>  \n<p>Viettel offers telecommunications, IT solutions, cybersecurity, and digital transformation services, catering to both individual and enterprise clients.</p>  \n\n<h2>Industry Presence</h2>  \n<p>As a market leader, Viettel serves a vast customer base in Vietnam and has expanded its operations to over 10 countries worldwide.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Viettel invests in 5G technology, AI, and IoT to drive innovation in telecommunications and support Vietnam’s digital economy.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Viettel continues to expand globally, focusing on emerging markets while maintaining its dominance in Vietnam’s telecom sector.</p>  \n\n<h2>Commitment to Society</h2>  \n<p>Viettel is dedicated to bridging the digital divide, supporting education, and contributing to Vietnam’s socioeconomic development through technology.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/d/d5/Viettel_Group_en_logo.svg",
                Industry = "Telecommunications, IT Solutions",
                FoundedDate = new DateTime(1989, 6, 1),
                Size = CompanySizes.Large,
                ProvinceCode = 1,
                WardCode = 166
            },
            new Company
            {
                Name = "FLC Group",
                Website = "https://flc.vn",
                Email = "info@flc.vn",
                Phone = "+84 24 3771 1111",
                StreetAddress = "FLC Landmark Tower, Le Duc Tho Street",
                Description =
                    "<h1>About FLC Group</h1>  \n\n<h2>Overview</h2>  \n<p>FLC Group is a leading real estate developer in Vietnam, known for its luxury resorts, hotels, and residential projects.</p>  \n\n<h2>Core Services</h2>  \n<p>FLC develops high-end real estate projects, including resorts, golf courses, commercial properties, and residential complexes.</p>  \n\n<h2>Industry Presence</h2>  \n<p>With a portfolio of iconic properties across Vietnam, FLC is a trusted name in the real estate and hospitality sectors.</p>  \n\n<h2>Innovation in Development</h2>  \n<p>FLC integrates smart technologies and sustainable practices into its projects, creating modern, eco-friendly living spaces.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>FLC continues to expand its footprint with new developments in key tourist destinations and urban centers across Vietnam.</p>  \n\n<h2>Commitment to Luxury</h2>  \n<p>FLC is committed to delivering world-class real estate projects that enhance lifestyles and contribute to Vietnam’s tourism industry.</p>  \n",
                Logo = "https://www.flc.vn/wp-content/themes/flc/assets/images/home/header-logo.png",
                Industry = "Real Estate, Hospitality",
                FoundedDate = new DateTime(2001, 10, 25),
                Size = CompanySizes.Large,
                ProvinceCode = 1,
                WardCode = 592
            },
            new Company
            {
                Name = "Lazada Vietnam",
                Website = "https://www.lazada.vn",
                Email = "support@lazada.vn",
                Phone = "+84 1900 1007",
                StreetAddress = "Saigon Centre, 67 Le Loi Street",
                Description =
                    "<h1>About Lazada Vietnam</h1>  \n\n<h2>Overview</h2>  \n<p>Lazada Vietnam is a leading e-commerce platform, offering a vast selection of products ranging from electronics to fashion and home goods.</p>  \n\n<h2>Core Services</h2>  \n<p>Lazada provides an online marketplace, logistics services, and payment solutions, making online shopping convenient and accessible.</p>  \n\n<h2>Industry Presence</h2>  \n<p>Lazada is a key player in Vietnam’s e-commerce landscape, competing with platforms like Shopee and Tiki while serving millions of customers.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Lazada uses AI, data analytics, and machine learning to optimize user experiences, improve logistics, and offer personalized shopping recommendations.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Since launching in Vietnam, Lazada has grown rapidly, expanding its product offerings and enhancing its delivery network to reach more customers.</p>  \n\n<h2>Commitment to Customers</h2>  \n<p>Lazada is dedicated to providing a seamless shopping experience, competitive prices, and reliable delivery services to its users.</p>  \n",
                Logo = "https://img.lazcdn.com/g/tps/images/ims-web/TB1T7K2d8Cw3KVjSZFuXXcAOpXa.png",
                Industry = "E-commerce, Online Retail",
                FoundedDate = new DateTime(2012, 3, 1),
                Size = CompanySizes.Large,
                ProvinceCode = 2,
                WardCode = 26740
            },
            new Company
            {
                Name = "Tiki Corporation",
                Website = "https://tiki.vn",
                Email = "support@tiki.vn",
                Phone = "+84 1900 6035",
                StreetAddress = "52 Ut Tich Street",
                Description =
                    "<h1>About Tiki Corporation</h1>  \n\n<h2>Overview</h2>  \n<p>Tiki Corporation is a leading e-commerce platform in Vietnam, known for its fast delivery and wide range of products, including books, electronics, and more.</p>  \n\n<h2>Core Services</h2>  \n<p>Tiki offers an online marketplace, TikiNOW fast delivery, and a subscription service (TikiPRO) for premium benefits.</p>  \n\n<h2>Industry Presence</h2>  \n<p>Tiki is a trusted name in Vietnam’s e-commerce sector, competing with platforms like Shopee and Lazada while focusing on quality and customer satisfaction.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Tiki leverages advanced logistics technology, AI-driven recommendations, and a robust supply chain to ensure fast and reliable deliveries.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Tiki has expanded its services across Vietnam, introducing new categories and enhancing its delivery infrastructure to meet growing demand.</p>  \n\n<h2>Commitment to Quality</h2>  \n<p>Tiki is committed to providing authentic products, fast delivery, and excellent customer service, making online shopping a delightful experience.</p>  \n",
                Logo = "https://salt.tikicdn.com/ts/upload/0e/07/78/ee828743c9afa9792cf20d75995e134e.png",
                Industry = "E-commerce, Online Retail",
                FoundedDate = new DateTime(2010, 3, 1),
                Size = CompanySizes.Medium,
                ProvinceCode = 2,
                WardCode = 27559
            },
            new Company
            {
                Name = "Axon Active Vietnam",
                Website = "https://www.axonactive.com",
                Email = "contact@axonactive.com",
                Phone = "+84 236 395 6789",
                StreetAddress = "11th Floor, PVcomBank Building 2, 30/4 Street",
                Description =
                    "<h1>About Axon Active Vietnam</h1>  \n\n<h2>Overview</h2>  \n<p>Axon Active Vietnam is a software development company specializing in offshore development, IT consulting, and digital solutions.</p>  \n\n<h2>Core Services</h2>  \n<p>Axon Active provides custom software development, mobile app development, and IT staffing services for global clients.</p>  \n\n<h2>Industry Presence</h2>  \n<p>With offices in Đà Nẵng and other cities, Axon Active serves international clients, particularly in Europe and the US, delivering high-quality software solutions.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Axon Active uses modern technologies like .NET, Java, React, and cloud platforms to build scalable and innovative software products.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Axon Active continues to grow its team and expand its services, focusing on talent development and global partnerships.</p>  \n\n<h2>Commitment to Excellence</h2>  \n<p>Axon Active is dedicated to delivering top-notch software solutions, fostering a collaborative work environment, and supporting client success.</p>  \n",
                Logo = "https://media.licdn.com/dms/image/v2/D560BAQHX8XJc-8-10A/company-logo_200_200/company-logo_200_200/0/1723627527429/axonactive_logo?e=2147483647&v=beta&t=Cn91Z_201LuzWuq5szyBR0sT3lFAkg5WINw2SMTo1SQ",
                Industry = "Software Development, IT Services",
                FoundedDate = new DateTime(2008, 1, 1),
                Size = CompanySizes.Medium,
                ProvinceCode = 48,
                WardCode = 20257
            },
            new Company
            {
                Name = "Phuc Long Tea & Coffee",
                Website = "https://phuclong.com.vn",
                Email = "support@phuclong.com",
                Phone = "+84 28 3821 2345",
                StreetAddress = "7th floor, Central Plaza, 17 Lê Duẩn",
                Description =
                    "<h1>About Phuc Long Tea & Coffee</h1>  \n\n<h2>Overview</h2>  \n<p>Phuc Long Tea & Coffee is a popular Vietnamese coffee and tea chain, known for its traditional flavors and modern café experience.</p>  \n\n<h2>Signature Offerings</h2>  \n<p>Phuc Long serves authentic Vietnamese iced coffee, milk tea, and a variety of herbal teas, focusing on quality ingredients.</p>  \n\n<h2>Store Experience</h2>  \n<p>Phuc Long stores offer a modern and inviting atmosphere, perfect for casual meetups, work, or relaxation by the beach in Đà Nẵng.</p>  \n\n<h2>Commitment to Quality</h2>  \n<p>Phuc Long sources its tea leaves and coffee beans from Vietnam’s finest regions, ensuring rich and authentic flavors in every drink.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Phuc Long has expanded across Vietnam and into international markets, bringing Vietnamese tea and coffee culture to a global audience.</p>  \n\n<h2>Community Engagement</h2>  \n<p>Phuc Long promotes Vietnamese heritage through its beverages, supporting local farmers and fostering cultural appreciation.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/vi/3/32/Logo_Ph%C3%BAc_Long.svg",
                Industry = "Food & Beverage, Coffee Retail",
                FoundedDate = new DateTime(2007, 1, 1),
                Size = CompanySizes.Medium,
                ProvinceCode = 2,
                WardCode = 26740
            }
        };
    }
}