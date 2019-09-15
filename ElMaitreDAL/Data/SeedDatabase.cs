using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.DAL.Data
{
    public class SeedDatabase
    {
        public async static void Initialize(IServiceProvider serviceProvider,bool reCreateDB)
        {
            if (reCreateDB)
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();


                var res = await roleManager.CreateAsync(new IdentityRole { Name = "Lawyer" });
                await roleManager.CreateAsync(new IdentityRole { Name = "User" });
                await roleManager.CreateAsync(new IdentityRole { Name = "Anonymous" });

                ApplicationUser user = new ApplicationUser
                {
                    Email = "Islam.ibrahimh@hotmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "AmrZahran",
                    NameEn = "Amr Zahran",
                    Name = "عمرو زهران",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1989, 1, 13),
                    EmailConfirmed = true
                };
                var s = await userManager.CreateAsync(user, "P@$$w0rd");

                ApplicationUser user2 = new ApplicationUser
                {
                    Email = "Islam@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "MortadaMansour",
                    Name = "مرتضي منصور",
                    NameEn = "Mortada Mansour",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1985, 1, 13),
                    EmailConfirmed = true
                };

                var ss = await userManager.CreateAsync(user2, "P@$$w0rd");

                ApplicationUser user3 = new ApplicationUser
                {
                    Email = "Islam.ibrahim1989g@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "Islam",
                    Name = "اسلام ابراهيم",
                    NameEn = "Islam elsayed",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(1985, 1, 13),
                    EmailConfirmed = true
                };

                var sss = await userManager.CreateAsync(user3, "P@$$w0rd");


                ApplicationUser anonymousUser = new ApplicationUser
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "anonymous",
                    Email = "anonymous@gmail.com",
                    EmailConfirmed = true,
                    Name = "Anonymous",
                    NameEn = "Anonymous"
                };

                var a = await userManager.CreateAsync(anonymousUser, "Anonymous@P@$$w0rd123");

                await userManager.AddToRoleAsync(user, "Lawyer");
                await userManager.AddToRoleAsync(user2, "Lawyer");
                await userManager.AddToRoleAsync(user3, "User");
                await userManager.AddToRoleAsync(anonymousUser, "Anonymous");






                context.LawyerSpecializations.AddRange(
                    new Models.LawyerSpecialization { Name = "قضايا الاحوال الشخصية و الاسرة", NameEn = "قضايا الاحوال الشخصية و الاسرة" },
                    new Models.LawyerSpecialization { Name = "قضايا القانون المدني", NameEn = "قضايا القانون المدني" },
                    new Models.LawyerSpecialization { Name = "قضايا القانون التجاري", NameEn = "قضايا القانون التجاري" },
                    new Models.LawyerSpecialization { Name = "قضايا العمال امام القضاء العمالي", NameEn = "قضايا العمال امام القضاء العمالي" },
                    new Models.LawyerSpecialization { Name = "قضايا مجلس الدولة", NameEn = "قضايا مجلس الدولة" },
                    new Models.LawyerSpecialization { Name = "القضاء العسكري", NameEn = "القضاء العسكري" },
                    new Models.LawyerSpecialization { Name = "العقود و الشهر العقاري", NameEn = "العقود و الشهر العقاري" },
                    new Models.LawyerSpecialization { Name = "قضايا القانون الجنائي", NameEn = "قضايا القانون الجنائي" },
                    new Models.LawyerSpecialization { Name = "القانون الرياضي", NameEn = "القانون الرياضي" },
                    new Models.LawyerSpecialization { Name = "قضايا التحكيم الدولي", NameEn = "قضايا التحكيم الدولي" },
                    new Models.LawyerSpecialization { Name = "القانون البحري و الجوي و الملاحة الدولية", NameEn = "القانون البحري و الجوي و الملاحة الدولية" },
                    new Models.LawyerSpecialization { Name = "قضايا الاموال العامة و غسيل الاموال", NameEn = "قضايا الاموال العامة و غسيل الاموال" },
                    new Models.LawyerSpecialization { Name = "جرائم امن الدولة", NameEn = "جرائم امن الدولة" },
                    new Models.LawyerSpecialization { Name = "قضايا الدستورية", NameEn = "قضايا الدستورية" },
                    new Models.LawyerSpecialization { Name = "حقوق الملكية الفكرية و براءات الاختراع و تسجيل العلامات و الوكالات", NameEn = "حقوق الملكية الفكرية و براءات الاختراع و تسجيل العلامات و الوكالات" },
                    new Models.LawyerSpecialization { Name = "قانون العمل و التأمينات و الضرائب", NameEn = "قانون العمل و التأمينات و الضرائب" },
                    new Models.LawyerSpecialization { Name = "قضايا البورصة و الاوراق المالية و المصرفية", NameEn = "قضايا البورصة و الاوراق المالية و المصرفية" },
                    new Models.LawyerSpecialization { Name = "الجنسية و الهجرة و مراكز الاجانب في مصر", NameEn = "الجنسية و الهجرة و مراكز الاجانب في مصر" },
                    new Models.LawyerSpecialization { Name = "تنفيذ الاحكام القضائية و التحكيمية داخل و خارج مصر", NameEn = "تنفيذ الاحكام القضائية و التحكيمية داخل و خارج مصر" },
                    new Models.LawyerSpecialization { Name = "تأسيس الشركات الاستثمارية في مصر", NameEn = "تأسيس الشركات الاستثمارية في مصر" },
                    new Models.LawyerSpecialization { Name = "التركات و تصفيتها و قسمتها", NameEn = "التركات و تصفيتها و قسمتها" },
                    new Models.LawyerSpecialization { Name = "الصلح و تسوية المنازعات بالطرق الودية", NameEn = "الصلح و تسوية المنازعات بالطرق الودية" }
                );

                context.LawyerExperiences.AddRange(
                    new Models.LawyerExperience { Name = "محامي نقض", NameEn = "Lawyer vetoed" },
                    new Models.LawyerExperience { Name = "محامي استاذ", NameEn = "Lawyer Professor" }
                );

                context.Countries.Add(new Models.Country { Name = "Egypt" });

                context.SaveChanges();


                context.Lawyers.AddRange(
                    new Models.Lawyer
                    {
                        UserId = user.Id,
                        Fees = 250,
                        Rate = 2,
                        Specialization = context.LawyerSpecializations.First(),
                        Description = @"ويختص بالمرافعة أمام هذه المحكمة المحامون المقيدون بجداول محكمة النقض دون غيرهم من المحامين وهم الذين يحركون هذه المحكمة لالفانها إلى هذا الخطأ في القانون أو القصور في تسبيب الأحكام أو الفساد في الاستدلال وفي بعض الحالات يكون ذلك واجبا على النيابة العامة في حالة الحكم علي المتهم بالإعدام وذلك لخطورة هذا الحكم فيكون على محكمة النقض أن تراقب هذا الحكم لتتأكد من سلامته أي أنها من يلفت محكمة النقض في هذه الحالة وأحيانا يكون جائزا للنيابة في حالة الحكم على المتهم بالبراءة.",
                        Certificates = @"ويختص بالمرافعة أمام هذه المحكمة المحامون المقيدون بجداول محكمة النقض دون غيرهم من المحامين وهم الذين يحركون هذه المحكمة لالفانها إلى هذا الخطأ في القانون أو القصور في تسبيب الأحكام أو الفساد في الاستدلال وفي بعض الحالات يكون ذلك واجبا على النيابة العامة في حالة الحكم علي المتهم بالإعدام وذلك لخطورة هذا الحكم فيكون على محكمة النقض أن تراقب هذا الحكم لتتأكد من سلامته أي أنها من يلفت محكمة النقض في هذه الحالة وأحيانا يكون جائزا للنيابة في حالة الحكم على المتهم بالبراءة.",
                        DescriptionEn = @"In this case, the lawyers who are bound by the Court of Cassation have no other lawyers than the others who are moving this court to its faults in the law or the failure to cause judgments or corruption in the inference. In some cases, The Court of Cassation must monitor this provision to ensure its safety, that is, it draws the Court of Cassation in this case and sometimes it is permissible for the prosecution in the case of the judgment of the accused of acquittal..
                                        In laoreet dolor tincidunt enim laoreet, sit amet volutpat nisi laoreet.",
                        CertificatesEn = @"In this case, the lawyers who are bound by the Court of Cassation have no other lawyers than the others who are moving this court to its faults in the law or the failure to cause judgments or corruption in the inference. In some cases, The Court of Cassation must monitor this provision to ensure its safety, that is, it draws the Court of Cassation in this case and sometimes it is permissible for the prosecution in the case of the judgment of the accused of acquittal..
                                        In laoreet dolor tincidunt enim laoreet, sit amet volutpat nisi laoreet.",
                        Experience = context.LawyerExperiences.First(),
                    },
                    new Models.Lawyer
                    {
                        UserId = user2.Id,
                        Fees = 300,
                        Rate = 4,
                        Specialization = context.LawyerSpecializations.Skip(1).First(),
                        Description = @"ويختص بالمرافعة أمام هذه المحكمة المحامون المقيدون بجداول محكمة النقض دون غيرهم من المحامين وهم الذين يحركون هذه المحكمة لالفانها إلى هذا الخطأ في القانون أو القصور في تسبيب الأحكام أو الفساد في الاستدلال وفي بعض الحالات يكون ذلك واجبا على النيابة العامة في حالة الحكم علي المتهم بالإعدام وذلك لخطورة هذا الحكم فيكون على محكمة النقض أن تراقب هذا الحكم لتتأكد من سلامته أي أنها من يلفت محكمة النقض في هذه الحالة وأحيانا يكون جائزا للنيابة في حالة الحكم على المتهم بالبراءة.",
                        Certificates = @"ويختص بالمرافعة أمام هذه المحكمة المحامون المقيدون بجداول محكمة النقض دون غيرهم من المحامين وهم الذين يحركون هذه المحكمة لالفانها إلى هذا الخطأ في القانون أو القصور في تسبيب الأحكام أو الفساد في الاستدلال وفي بعض الحالات يكون ذلك واجبا على النيابة العامة في حالة الحكم علي المتهم بالإعدام وذلك لخطورة هذا الحكم فيكون على محكمة النقض أن تراقب هذا الحكم لتتأكد من سلامته أي أنها من يلفت محكمة النقض في هذه الحالة وأحيانا يكون جائزا للنيابة في حالة الحكم على المتهم بالبراءة.",
                        DescriptionEn = @"In this case, the lawyers who are bound by the Court of Cassation have no other lawyers than the others who are moving this court to its faults in the law or the failure to cause judgments or corruption in the inference. In some cases, The Court of Cassation must monitor this provision to ensure its safety, that is, it draws the Court of Cassation in this case and sometimes it is permissible for the prosecution in the case of the judgment of the accused of acquittal..
                                        In laoreet dolor tincidunt enim laoreet, sit amet volutpat nisi laoreet.",
                        CertificatesEn = @"In this case, the lawyers who are bound by the Court of Cassation have no other lawyers than the others who are moving this court to its faults in the law or the failure to cause judgments or corruption in the inference. In some cases, The Court of Cassation must monitor this provision to ensure its safety, that is, it draws the Court of Cassation in this case and sometimes it is permissible for the prosecution in the case of the judgment of the accused of acquittal..
                                        In laoreet dolor tincidunt enim laoreet, sit amet volutpat nisi laoreet.",
                        Experience = context.LawyerExperiences.Skip(1).First(),
                    }
                    );




                //context.Appointments.AddRange(new Models.LawyerAppointment { LawyerId = 1, Time = new TimeSpan(15, 10, 0), Date = new DateTime(2018, 7, 16) });
                //context.Appointments.AddRange(new Models.LawyerAppointment { LawyerId = 1, Time = new TimeSpan(23, 10, 0), Date = new DateTime(2018, 7, 15) });
                //context.Appointments.AddRange(new Models.LawyerAppointment { LawyerId = 1, Time = new TimeSpan(12, 10, 0), Date = new DateTime(2018, 7, 13) });
                //context.Appointments.AddRange(new Models.LawyerAppointment { LawyerId = 1, Time = new TimeSpan(19, 10, 0), Date = new DateTime(2018, 7, 1) });



                //context.Reviews.AddRange(new Models.Review { LawyerId = 1, Rate = 4, Title = "good lawyer",UserId=user3.Id });


                //context.Reviews.AddRange(new Models.Review { LawyerId = 2, Rate = 4, Title = "good lawyer", UserId = user3.Id });



                context.PriceRanges.AddRange(new Models.PriceRange { From = 0, To = 200 },
                    new Models.PriceRange { From = 200, To = 400 },
                    new Models.PriceRange { From = 400, To = 600 },
                    new Models.PriceRange { From = 600, To = 1000 });



                context.QuestionCategories.AddRange(
                    new Models.QuestionCategory { Title = "قضايا الاحوال الشخصية و الاسرة", TitleEn = "قضايا الاحوال الشخصية و الاسرة" },
                    new Models.QuestionCategory { Title = "قضايا القانون المدني", TitleEn = "قضايا القانون المدني" },
                    new Models.QuestionCategory { Title = "قضايا القانون التجاري", TitleEn = "قضايا القانون التجاري" },
                    new Models.QuestionCategory { Title = "قضايا العمال امام القضاء العمالي", TitleEn = "قضايا العمال امام القضاء العمالي" },
                    new Models.QuestionCategory { Title = "قضايا مجلس الدولة", TitleEn = "قضايا مجلس الدولة" },
                    new Models.QuestionCategory { Title = "القضاء العسكري", TitleEn = "القضاء العسكري" },
                    new Models.QuestionCategory { Title = "العقود و الشهر العقاري", TitleEn = "العقود و الشهر العقاري" },
                    new Models.QuestionCategory { Title = "قضايا القانون الجنائي", TitleEn = "قضايا القانون الجنائي" },
                    new Models.QuestionCategory { Title = "القانون الرياضي", TitleEn = "القانون الرياضي" },
                    new Models.QuestionCategory { Title = "قضايا التحكيم الدولي", TitleEn = "قضايا التحكيم الدولي" },
                    new Models.QuestionCategory { Title = "القانون البحري و الجوي و الملاحة الدولية", TitleEn = "القانون البحري و الجوي و الملاحة الدولية" },
                    new Models.QuestionCategory { Title = "قضايا الاموال العامة و غسيل الاموال", TitleEn = "قضايا الاموال العامة و غسيل الاموال" },
                    new Models.QuestionCategory { Title = "جرائم امن الدولة", TitleEn = "جرائم امن الدولة" },
                    new Models.QuestionCategory { Title = "قضايا الدستورية", TitleEn = "قضايا الدستورية" },
                    new Models.QuestionCategory { Title = "حقوق الملكية الفكرية و براءات الاختراع و تسجيل العلامات و الوكالات", TitleEn = "حقوق الملكية الفكرية و براءات الاختراع و تسجيل العلامات و الوكالات" },
                    new Models.QuestionCategory { Title = "قانون العمل و التأمينات و الضرائب", TitleEn = "قانون العمل و التأمينات و الضرائب" },
                    new Models.QuestionCategory { Title = "قضايا البورصة و الاوراق المالية و المصرفية", TitleEn = "قضايا البورصة و الاوراق المالية و المصرفية" },
                    new Models.QuestionCategory { Title = "الجنسية و الهجرة و مراكز الاجانب في مصر", TitleEn = "الجنسية و الهجرة و مراكز الاجانب في مصر" },
                    new Models.QuestionCategory { Title = "تنفيذ الاحكام القضائية و التحكيمية داخل و خارج مصر", TitleEn = "تنفيذ الاحكام القضائية و التحكيمية داخل و خارج مصر" },
                    new Models.QuestionCategory { Title = "تأسيس الشركات الاستثمارية في مصر", TitleEn = "تأسيس الشركات الاستثمارية في مصر" },
                    new Models.QuestionCategory { Title = "التركات و تصفيتها و قسمتها", TitleEn = "التركات و تصفيتها و قسمتها" },
                    new Models.QuestionCategory { Title = "الصلح و تسوية المنازعات بالطرق الودية", TitleEn = "الصلح و تسوية المنازعات بالطرق الودية" }
);




                context.ContractCategories.AddRange(new Models.ContractCategory { Name = "فئة 1", NameEn = "Category 1" },
                    new Models.ContractCategory { Name = "فئة 2", NameEn = "Category 2" },
                    new Models.ContractCategory { Name = "فئة 3", NameEn = "Category 3" },
                    new Models.ContractCategory { Name = "فئة 4", NameEn = "Category 4" });


                context.SaveChanges();



                context.Contracts.AddRange(new Models.Contract
                {
                    Name = "عقد 1",
                    NameEn = "contract1",
                    CategoryId = 1,
                    Description = "تفاصيل العقد 1",
                    DescriptionEn = "Details of contract 1",
                    Path = "uploads/2.pdf"
                },
                new Models.Contract
                {
                    Name = "عقد 2",
                    NameEn = "contract 2",
                    CategoryId = 2,
                    Description = "تفاصيل العقد 2",
                    DescriptionEn = "Details of contract 2",
                    Path = "uploads/1.pdf"
                },
                new Models.Contract
                {
                    Name = "عقد 3",
                    NameEn = "contract 3",
                    CategoryId = 2,
                    Description = "تفاصيل العقد 3",
                    DescriptionEn = "Details of contract 3",
                    Path = "uploads/1.pdf"
                }
                );


                context.Blogs.AddRange(new Models.Blog
                {
                    Name = "بلوج 1",
                    NameEn = "blog 1",
                    Description = "تفاصيل بلوج 1",
                    DescriptionEn = "Details of blog 1",
                    ImagePath = "uploads/1.jpg"
                },
                new Models.Blog
                {
                    Name = "بلوج 2",
                    NameEn = "blog 2",
                    Description = "تفاصيل بلوج 2",
                    DescriptionEn = "Details of blog 2",
                    ImagePath = "uploads/1.jpg"
                },
                new Models.Blog
                {
                    Name = "بلوج 3",
                    NameEn = "blog 3",
                    Description = "تفاصيل بلوج 3",
                    DescriptionEn = "Details of blog 3",
                    ImagePath = "uploads/1.jpg"
                }
                );




                //context.Documents.AddRange(new Models.Document
                //{
                //    Name = "Document 1",
                //    FromUserId = user.Id,
                //    ToUserId = user3.Id,
                //    Path = "uploads/document1.jpg",
                //},
                //new Models.Document
                //{
                //    Name = "Document 2",
                //    FromUserId = user3.Id,
                //    ToUserId = user.Id,
                //    Path = "uploads/document1.jpg",
                //}
                //);

                var cid = context.Countries.First().Id;
                context.Provinces.AddRange(
                    new Models.Province { Name = "محافظة الاسكندرية", NameEn = "Alexandria Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة القاهرة", NameEn = "Cairo Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة الإسماعيلية", NameEn = "Ismailia Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة أسوان", NameEn = "Aswan Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة أسيوط", NameEn = "Assiut Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة الأقصر", NameEn = "Luxor Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة البحر الأحمر", NameEn = "Red Sea Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة البحيرة", NameEn = "Lake Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة بني سويف", NameEn = "Beni Suef Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة بورسعيد", NameEn = "Port Said Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة جنوب سيناء", NameEn = "South Sinai Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة الجيزة", NameEn = "Giza Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة الدقهلية", NameEn = "Dakahlia Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة دمياط", NameEn = "Damietta Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة سوهاج", NameEn = "Sohag Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة السويس", NameEn = "Suez Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة الشرقية", NameEn = "Eastern Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة شمال سيناء", NameEn = "North Sinai Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة الغربية", NameEn = "Western Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة الفيوم", NameEn = "Fayoum Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة القليوبية", NameEn = "Kalyubia Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة قنا", NameEn = "Qena Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة كفر الشيخ", NameEn = "Kafr El-Sheikh Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة مطروح", NameEn = "Matrouh Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة المنوفية", NameEn = "Monofia Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة المنيا", NameEn = "Minia Governorate", CountryId = cid },
                    new Models.Province { Name = "محافظة الوادي الجديد", NameEn = "New Valley Governorate", CountryId = cid }
                );


                string papers = @"1- اسم المحامي ثلاثي عشان الموكل يعمل التوكيل ,
2 - التوكيل في حالة تاسيس الشركات يكون توكيل رسمي عام بالقضايا + تاسيس الشركات والتعامل مع الغرفة التجارية والسجل التجاري والضرائب
اما في حالة اي قضية غير تاسيس الشركات يكون التوكيل رسمي عام بالقضايا وبعد كده من حق الموكل يستلم اصل التوكيل بعد الانتهاء من القضية او الدعوي ,
3 - صورة ضوئية من بطاقة الرقم القومي لصاحب التوكيل(الموكل) تكون سارية وغير منتهية";




                context.ServiceCategories.AddRange(
                    new Models.ServiceCategory
                    {
                        NameEn = "قضايا الاحوال الشخصية و الاسرة",
                        Name = "قضايا الاحوال الشخصية و الاسرة",
                        IconPath = "images/icons/a7wal.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="نفقة الزوجية ونفقة الصغار" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                            Prices=new List<Models.ServicePrice>{
                                new Models.ServicePrice{ Price=2000, ExperienceId=1 },
                                new Models.ServicePrice{ Price=2500, ExperienceId=2 },
                            }
                          },
                             new Models.Service{ Title="الاجور وما فى حكمه أجر حضانة وأجر مسكن وفرش وغطاء))" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",
                            Prices=new List<Models.ServicePrice>{
                                new Models.ServicePrice{ Price=1500, ExperienceId=1 },
                                new Models.ServicePrice{ Price=2000, ExperienceId=2 },
                            }
                          },
                             new Models.Service{ Title="ضم صغير كقرار من المحامى العام" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",
                            Prices=new List<Models.ServicePrice>{
                                new Models.ServicePrice{ Price=2500, ExperienceId=1 },
                            }
                        },
                             new Models.Service{ Title="ضم صغير كقرار من المحامى العام" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",
                            Prices=new List<Models.ServicePrice>{
                                new Models.ServicePrice{ Price=2500, ExperienceId=1 },
                            }
                        },

                        new Models.Service{ Title="ضم صغير دعوى فى محكمة الاسرة" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3500, ExperienceId=1 }}},
                        new Models.Service{ Title="ضم صغير كقرار من المحامى العام" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2500, ExperienceId=1 }}},
                        new Models.Service{ Title="اثبات الزواج أو اثبات العلاقة الزوجية" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2500, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }}},
                        new Models.Service{ Title="عدم صلاحية حضانة أو اسقاط حضانة" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3500, ExperienceId=1 }}},
                        new Models.Service{ Title="زيادة أو تخفيض مفروض من نفقة" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}},
                        new Models.Service{ Title="مطالبة بمؤخر صداق" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 }}},
                        new Models.Service{ Title="نفقة العدة " , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}},
                        new Models.Service{ Title="نفقة المتعة" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}},
                        new Models.Service{ Title="مطالبة قائمة منقولات" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=1500, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }} },
                        new Models.Service{ Title="اعلام وراثة" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=1000, ExperienceId=1 }}},
                        new Models.Service{ Title="نشوز الزوجة" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }}},
                        new Models.Service{ Title="الطلاق" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }}},
                        new Models.Service{ Title="الخلع" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=4000, ExperienceId=1 }}},
                        new Models.Service{ Title="حبس نفقة" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 }}},
                        new Models.Service{ Title="انذار طاعة" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=500, ExperienceId=1 }}},
                        new Models.Service{ Title="قرارات القوامة والوصاية والحجر والولاية التعليمية" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=800, ExperienceId=1 }}},
                        new Models.Service{ Title="التركات وتصفيتها وقسمتها وتوزيعها" , PaperWork=papers,ContactNumber="٠١٠١٦١١٠٠١٠",Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price= 10000, ExperienceId=1 }}}
                      }
                    },
                    new Models.ServiceCategory
                    {
                        NameEn = "قضايا القانون المدني",
                        Name = "قضايا القانون المدني",
                        IconPath = "images/icons/kanonmadany.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="قرارات التمكين من المحامى العام" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}
                            },
                           new Models.Service{ Title="فسخ عقد بيع" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=2500, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}
                            },
                            new Models.Service{ Title="صحة العرض والزام المشترى بدفع الثمن" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}
                            },
                            new Models.Service{ Title="امتداد علاقة ايجارية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=4000, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="دعاوى الايجارات والطرد والاخلاء وجميع النزاعات فى الايجارات" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}
                            },
                            new Models.Service{ Title="قضايا المحكمة العمالية " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}
                            },


                            new Models.Service{ Title="دعاوى اثبات الحالة " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=3000, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="فرز وتجنيب وقسمة  " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=8000, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="ثبوت ملكية بوضع اليد " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}
                            },
                            new Models.Service{ Title="شفعة " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=3000, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="تعويضات مدنية " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=4000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}
                            },
                            new Models.Service{ Title="صحة ونفاذ عقد البيع " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=10000, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="صحة توقيع على جميع العقود " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=2000, ExperienceId=1 }}
                            },
                             new Models.Service{ Title="عدم نفاذ البيع فى حق المالك " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=4000, ExperienceId=1 }}
                            },

                            new Models.Service{ Title="دعاوى المطالبات المدنية " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }}
                            },
                            new Models.Service{ Title="دعاوى الاسترداد " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}
                            },
                            new Models.Service{ Title="دعاوى التكليف بالوفاء " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }}
                            },
                            new Models.Service{ Title="جميع الانذارات بعرض قيم أو اداء قيمة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=600, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="جميع اشكالات التنفيذ ووقف تنفيذ الاحكام ومباشرتها موضوعيا " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=3500, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="جميع اجراءات  انذارات الصرف فى أقسام الودائع فى المحاكم المدنية حتى الاستلام " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=3000, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="صحة ونفاذ عقد البيعجميع النزاعات المدنية فى المحاكم المدنية على الاراضى والعقارات " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=10000, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="دعاوى انهاء عقود الايجار سواء للغلق أو لتغيير النشاط أولوفاة المستأجر " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}
                            },
                            new Models.Service{ Title="تقارير الخبراء وأماناتها والطب الشرعى ومتابعتها  " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=2500, ExperienceId=1 }}
                            },
                    }
                    },
                    new Models.ServiceCategory
                    {
                        NameEn = "العقود و الشهر العقاري",
                        Name = "العقود و الشهر العقاري",
                        IconPath = "images/icons/aqari.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="عقد ايجار " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠",
                                Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=600, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="عقد تمليك أو بيع نهائى" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{
                                    new Models.ServicePrice{ Price=1000, ExperienceId=1 }}
                            },
                            new Models.Service{ Title="عقود التنازل بكافة أنواعها " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=1000, ExperienceId=1 }}},
                            new Models.Service{ Title="عقود الهبة والانتفاع الخ" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=1000, ExperienceId=1 }}},
                            new Models.Service{ Title="عقود الاتفاق" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=750, ExperienceId=1 }}},
                            new Models.Service{ Title="عقود العمل" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=1000, ExperienceId=1 }}},
                            new Models.Service{ Title="العقود التجارية كافة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 }}},
                            new Models.Service{ Title="العقود الادارية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=1000, ExperienceId=1 }}},
                            new Models.Service{ Title="العقود الهندسية والمعمارية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 }}},
                            new Models.Service{ Title="عقود المقاولات" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2000, ExperienceId=1 }}},
                            new Models.Service{ Title="التوثيقات والتصديقات بالشهر العقارى" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=1500, ExperienceId=1 }}},
                        }
                    },
                    new Models.ServiceCategory
                    {
                        NameEn = "القضايا الجنائية والجنــــــح",
                        Name = "القضايا الجنائية والجنــــــح",
                        IconPath = "images/icons/criminal-icon.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="المخدرات جلب وتهريب" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=30000, ExperienceId=1 }}},
                            new Models.Service{ Title="المخدرات اتجار" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=15000, ExperienceId=1 }}},
                            new Models.Service{ Title="المخدرات تعاطى" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                            new Models.Service{ Title="جرائم القتل العمد" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=30000, ExperienceId=1 }}},
                            new Models.Service{ Title="جرائم القتل الخطأ" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},

                            new Models.Service{ Title="جرائم ضرب أفضى الى موت" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=15000, ExperienceId=1 }}},
                            new Models.Service{ Title="جرائم أمن الدولة العليا" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=20000, ExperienceId=1 }}},
                            new Models.Service{ Title="الاموال العامة والاختلاسات" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=25000, ExperienceId=1 }}},
                            new Models.Service{ Title="جنح التبديد بالكامل" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنح النصب والاحتبال" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=4000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنح الاتلاف" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2500, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنح الضرب " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2500, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنح الاهمال الطبى أدى لأكثر من 30% عاهة مستديمة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=500, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنح الاهمال الطبى أدى الى  الوفاة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=5000, ExperienceId=1 },new Models.ServicePrice{ Price=10000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنحة امتناع عن تسليم صغير" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنحة سب وقذف" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنحة غيرمباشرة ايصال أمانة أوشيك بدون رصيد" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="الدعاوى المدنية فى الجنح" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2000, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنح مباشرة خيانة الامانة( ايصال أمانة / شيك)" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنايات وجنح التزويروالتزييف" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=5000, ExperienceId=1 },new Models.ServicePrice{ Price=5000, ExperienceId=2 }}},
                            new Models.Service{ Title="تعدى على أنثى" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=2000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنح الغش التجارى والصحة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=4000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنح السرقة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=2500, ExperienceId=2 }}},
                            new Models.Service{ Title="جنايات السرقة بالاكراة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=7000, ExperienceId=1 }}},
                            new Models.Service{ Title="السلاح الابيض والنارى" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=6000, ExperienceId=1 },new Models.ServicePrice{ Price=4000, ExperienceId=2 }}},
                            new Models.Service{ Title="جنح المبانى والمخالفات" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="جرائم الاحداث بكافة الجنح" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 },new Models.ServicePrice{ Price=3000, ExperienceId=2 }}},
                            new Models.Service{ Title="جرائم الاحداث بكافة الجنايات" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=8000, ExperienceId=1 }}},
                            new Models.Service{ Title="السطو المسلح وتكدير الامن العام " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=20000, ExperienceId=1 }}},
                            new Models.Service{ Title="حضور تحقيقات النيابة العامة فقط" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=2000, ExperienceId=1 }}},
                            new Models.Service{ Title="التصرفات القانونية فى النيابة واجراءاتها وتجديداتها" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=1000, ExperienceId=1 }}},
                        }
                    },
                    new Models.ServiceCategory
                    {
                        NameEn = "قضايا مجلس الدولة",
                        Name = "قضايا مجلس الدولة",
                        IconPath = "images/icons/magls.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="النيابة الادارية وتحقيقاتها" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=500, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا القضاء الادارى" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=4000, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا المحكمة الادارية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=4000, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا المحكمة التأديبية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا الادارية العليا " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=4000, ExperienceId=1 }}}
                        }
                    },
                    new Models.ServiceCategory
                    {
                        NameEn = "الجنسية و الهجرة",
                        Name = "الجنسية و الهجرة",
                        IconPath = "images/icons/natinoality.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="اكتساب الجنسية المصرية من أم مصرية " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=6000, ExperienceId=1 }}},
                            new Models.Service{ Title="اكتساب الجنسية المصرية للزواج بمصرى" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=5000, ExperienceId=1 }}},
                            new Models.Service{ Title="التخلى أو رد الجنسية المصرية للحصول على جنسية أخرى أجنبية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=5000, ExperienceId=1 }}},
                            new Models.Service{ Title="زواج الاجانب وتوثيقة فى وزارة العدل والسفارات المختلفة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 }}},
                            new Models.Service{ Title="الاقامات المؤقتة والتصاريح الصادرة من الجوازات والهجرة للأجانب" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=1500, ExperienceId=1 }}}
                        }
                    },
                    new Models.ServiceCategory
                    {
                        NameEn = "تنفيذ الاحكام القضائية و التحكيمية داخل و خارج مصر",
                        Name = "تنفيذ الاحكام القضائية و التحكيمية داخل و خارج مصر",
                        IconPath = "images/icons/kada2.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="التنفيذ داخل مصر  للأحكام المصرية والاحكام الاجنبية المدنية " , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=5500, ExperienceId=1 }}},
                            new Models.Service{ Title="التنفيذ داخل مصر  للأحكام المصرية والاحكام الاجنبية الشرعية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 }}},
                            new Models.Service{ Title="التنفيذ داخل مصر  للأحكام المصرية والاحكام الاجنبية الجنائية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=8000, ExperienceId=1 }}},
                            new Models.Service{ Title="التنفيذ خارج مصر لجميع الاحكام المصرية سواء المدنية أو الشرعية أو الجنائية  عن طريق وزارة الخارجية والانتربول ومكتب التعاون الدولى من مكتب النائب العام" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=8000, ExperienceId=1 }}},
                        }
                    },
                    new Models.ServiceCategory
                    {
                        NameEn = "تاسيس الشركات",
                        Name = "تاسيس الشركات",
                        IconPath = "images/icons/employment-icon.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="الفردية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=5000, ExperienceId=1 }}},
                            new Models.Service{ Title="التوصية البسيطة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=7000, ExperienceId=1 }}},
                            new Models.Service{ Title="التضامن والشركات ذات المسئولية المحدودة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                            new Models.Service{ Title="المساهمة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=12000, ExperienceId=1 }}},
                            new Models.Service{ Title="(راس مال اكثر من 100 الف) الفردية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=7000, ExperienceId=1 }}},
                            new Models.Service{ Title="(راس مال اكثر من 100 الف) التوصية البسيطة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=9000, ExperienceId=1 }}},
                            new Models.Service{ Title="(راس مال اكثر من 100 الف) التضامن والشركات ذات المسئولية المحدودة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=12000, ExperienceId=1 }}},
                            new Models.Service{ Title="(راس مال اكثر من 100 الف) المساهمة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=14000, ExperienceId=1 }}},
                            new Models.Service{ Title="(غير تابعة لهيئة الاستثمار) الفردية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=4000, ExperienceId=1 }}},
                            new Models.Service{ Title="(غير تابعة لهيئة الاستثمار) التوصية البسيطة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=6000, ExperienceId=1 }}},
                            new Models.Service{ Title="(غير تابعة لهيئة الاستثمار) التضامن والشركات ذات المسئولية المحدودة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=8000, ExperienceId=1 }}},
                            new Models.Service{ Title="(غير تابعة لهيئة الاستثمار) المساهمة" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                            new Models.Service{ Title="الاجراءات الناشئة عن الخلاف بين الشركات" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                            new Models.Service{ Title="الاجراءات الناشئة عن الخلاف بين شركة والتأمينات والضرائب ومكتب العمل" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=4000, ExperienceId=1 }}},
                            new Models.Service{ Title="الاجراءات الخاصة بتشغيل المنشأة مثل التأمين على الموظفين والتراخيص كافة وغيرها" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 }}},

                        }
                    },
                    new Models.ServiceCategory
                    {
                        NameEn = "جميع قضايا القانون التجارى ",
                        Name = "جميع قضايا القانون التجارى",
                        IconPath= "images/icons/foreigners-icon.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="قضايا الشركات والنزاعات الناشئة عن العقود التجارية مع الشركات الاخرى والناشئة عن الاختلافات بين الشركاء" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا الاوراق التجارية ( الفواتير / بوالص الشحن)" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=7000, ExperienceId=1 }}},
                            new Models.Service{ Title="الاحتيال التجارى" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=7000, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا الوكالات التجارية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا الاعتراض والدفاع عن العلامات التجارية والاسماء التجارية للشركات" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=7000, ExperienceId=1 }}},
                            new Models.Service{ Title="حقوق الملكية الفكرية وبراءات الاختراع وتسجيل العلامات والوكالات التجارية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا الجمارك والتخليص الجمركى" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا الوساطة التجارية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=7000, ExperienceId=1 }}},
                            new Models.Service{ Title="قضايا الشحن الدولى والوكلاء التجاريين الدوليين" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                            new Models.Service{ Title="كل ما يتعلق بقانون العمل والتأمينات والضرائب" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=3000, ExperienceId=1 }}},

                        }
                    },
                    new Models.ServiceCategory
                    {
                        NameEn = "قضايا سوق المال والبورصة وتداول الاوراق المالية",
                        Name = "قضايا سوق المال والبورصة وتداول الاوراق المالية",
                        IconPath = "images/icons/commercial-icon.png",
                        Services = new List<Models.Service>
                        {
                            new Models.Service{ Title="عشرة الاف جنيه علي سبيل الاتعاب في جمع القضايا غير شاملة امانات الخبراء والرسوم الرسمية بايصالات رسمية" , PaperWork=papers, ContactNumber="٠١٠١٦١١٠٠١٠", Prices=new List<Models.ServicePrice>{new Models.ServicePrice{ Price=10000, ExperienceId=1 }}},
                        }
                    }

                    );


                context.SaveChanges();

                user.LawyerId = context.Lawyers.First().Id;
                user2.LawyerId = context.Lawyers.Skip(1).First().Id;
                await userManager.UpdateAsync(user);
                await userManager.UpdateAsync(user2);

            }
        }

    }
}
