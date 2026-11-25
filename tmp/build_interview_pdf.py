import sys
from pathlib import Path

SKILL = Path(r'C:\Users\Asus\.gapcode\plugins\cache\gapgpt\pdf\26.722.10000\skills\pdf')
sys.path.insert(0, str(SKILL / 'scripts'))
from fa_pdf import register_persian_fonts, rtl_paragraph, jalali_today_str

from reportlab.lib import colors
from reportlab.lib.enums import TA_RIGHT, TA_LEFT, TA_CENTER
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import mm
from reportlab.pdfbase.pdfmetrics import stringWidth
from reportlab.platypus import (SimpleDocTemplate, Paragraph, Spacer, Table, TableStyle,
                                PageBreak, KeepTogether, XPreformatted, HRFlowable)
from xml.sax.saxutils import escape

OUT = Path(r'C:\Users\Asus\source\repos\MVCSinglePage\راهنمای-مصاحبه-جونیور-دات-نت.pdf')
register_persian_fonts()
NAVY = colors.HexColor('#142B45')
TEAL = colors.HexColor('#078A91')
INK = colors.HexColor('#243448')
MUTED = colors.HexColor('#617286')
PALE = colors.HexColor('#EFF5F7')
LINE = colors.HexColor('#D5E0E6')
WHITE = colors.white

doc = SimpleDocTemplate(str(OUT), pagesize=A4, rightMargin=19*mm, leftMargin=19*mm,
                        topMargin=20*mm, bottomMargin=19*mm, title='راهنمای مصاحبه جونیور دات‌نت',
                        author='راهنمای آموزشی')
W = doc.width
styles = getSampleStyleSheet()
styles.add(ParagraphStyle(name='bodyfa', fontName='Vazirmatn', fontSize=9.4, leading=17,
                         textColor=INK, alignment=TA_RIGHT, spaceAfter=4))
styles.add(ParagraphStyle(name='smallfa', fontName='Vazirmatn', fontSize=8, leading=14,
                         textColor=MUTED, alignment=TA_RIGHT, spaceAfter=2))
styles.add(ParagraphStyle(name='h1fa', fontName='Vazirmatn-Bold', fontSize=20, leading=31,
                         textColor=NAVY, alignment=TA_RIGHT, spaceBefore=4, spaceAfter=9))
styles.add(ParagraphStyle(name='h2fa', fontName='Vazirmatn-Bold', fontSize=14, leading=23,
                         textColor=TEAL, alignment=TA_RIGHT, spaceBefore=10, spaceAfter=5))
styles.add(ParagraphStyle(name='h3fa', fontName='Vazirmatn-Bold', fontSize=10.5, leading=18,
                         textColor=NAVY, alignment=TA_RIGHT, spaceBefore=5, spaceAfter=2))
styles.add(ParagraphStyle(name='code', fontName='Courier', fontSize=7.0, leading=10,
                         textColor=colors.HexColor('#E8F1F8'), alignment=TA_LEFT,
                         leftIndent=5, rightIndent=5, spaceAfter=0))
styles.add(ParagraphStyle(name='label', fontName='Vazirmatn-Bold', fontSize=8.5, leading=14,
                         textColor=WHITE, alignment=TA_RIGHT))

S = styles
story = []
def p(text, style='bodyfa'):
    story.append(rtl_paragraph(text, S[style], W))
def title(text):
    p(text, 'h1fa')
    story.append(HRFlowable(width='100%', thickness=1, color=LINE, spaceAfter=9))
def section(text): p(text, 'h2fa')
def code(text):
    # Preserve source order and monospace alignment for C# snippets.
    block = XPreformatted(escape(text), S['code'])
    t = Table([[block]], colWidths=[W])
    t.setStyle(TableStyle([('BACKGROUND',(0,0),(-1,-1),NAVY),('BOX',(0,0),(-1,-1),.5,TEAL),
                           ('LEFTPADDING',(0,0),(-1,-1),9),('RIGHTPADDING',(0,0),(-1,-1),9),
                           ('TOPPADDING',(0,0),(-1,-1),8),('BOTTOMPADDING',(0,0),(-1,-1),8)]))
    story.extend([t, Spacer(1,5)])
def callout(label, text):
    left = Paragraph(escape(label), S['label'])
    right = rtl_paragraph(text, S['bodyfa'], W-30*mm)
    t=Table([[left,right]], colWidths=[24*mm,W-24*mm])
    t.setStyle(TableStyle([('BACKGROUND',(0,0),(0,0),TEAL),('BACKGROUND',(1,0),(1,0),PALE),
                          ('VALIGN',(0,0),(-1,-1),'MIDDLE'),('LEFTPADDING',(0,0),(-1,-1),7),
                          ('RIGHTPADDING',(0,0),(-1,-1),7),('TOPPADDING',(0,0),(-1,-1),7),
                          ('BOTTOMPADDING',(0,0),(-1,-1),7)]))
    story.extend([t,Spacer(1,7)])

def header_footer(canvas, d):
    canvas.saveState()
    w,h=A4
    if d.page > 1:
        canvas.setFillColor(NAVY); canvas.rect(0,h-11*mm,w,11*mm,fill=1,stroke=0)
        canvas.setFont('Vazirmatn-Bold',8)
        from fa_pdf import rtl_line
        canvas.setFillColor(WHITE); canvas.drawRightString(w-19*mm,h-7.3*mm,rtl_line('راهنمای آمادگی مصاحبهٔ جونیور دات‌نت'))
        canvas.setStrokeColor(LINE); canvas.line(19*mm,13*mm,w-19*mm,13*mm)
        canvas.setFont('Vazirmatn',7); canvas.setFillColor(MUTED)
        canvas.drawRightString(w-19*mm,8*mm,rtl_line(f'صفحهٔ {d.page}'))
    canvas.restoreState()

# Cover
story.append(Spacer(1,25*mm))
cover_title=ParagraphStyle('cover1',parent=S['h1fa'],textColor=WHITE,fontSize=18,leading=28)
cover=Table([[rtl_paragraph('آمادگی مصاحبهٔ شغلی', cover_title, W-20*mm)],
             [rtl_paragraph('توسعه‌دهندهٔ جونیور دات‌نت', ParagraphStyle('cover2',parent=S['h1fa'],fontSize=25,leading=37,textColor=WHITE), W-20*mm)],
             [rtl_paragraph('مفاهیم پایه، نمونه‌متدهای C#، تمرین API و پرسش‌وپاسخ', ParagraphStyle('cover3',parent=S['bodyfa'],fontSize=11,leading=20,textColor=colors.HexColor('#D6E6F2')), W-20*mm)]], colWidths=[W])
cover.setStyle(TableStyle([('BACKGROUND',(0,0),(-1,-1),NAVY),('BOX',(0,0),(-1,-1),0,NAVY),
                           ('LEFTPADDING',(0,0),(-1,-1),13*mm),('RIGHTPADDING',(0,0),(-1,-1),13*mm),
                           ('TOPPADDING',(0,0),(-1,0),13*mm),('BOTTOMPADDING',(0,0),(-1,-1),9*mm)]))
story.extend([cover,Spacer(1,11*mm)])
callout('روش مطالعه','هر پاسخ را با زبان خودتان بازگو کنید، بعد مثال کد را توضیح دهید. در مصاحبه، فهم دلیل تصمیم‌ها از حفظ کردن تعریف‌ها مهم‌تر است.')
p('این نسخه بر پایهٔ مسیر رایج ASP.NET Core MVC و EF Core آماده شده و نمونهٔ پروژهٔ شما را نیز در نظر می‌گیرد. نمونه‌ها آموزشی‌اند و ممکن است برای کامپایل به مدل‌ها و فضای نام پروژه نیاز داشته باشند.')
section('نقشهٔ راه')
for item in ['۱. مفاهیم C# و شی‌گرایی','۲. async/await، LINQ و نمونه‌متدها','۳. MVC، API و اعتبارسنجی','۴. EF Core و SQL','۵. پرسش‌های پرتکرار با پاسخ','۶. تمرین طراحی و برنامهٔ مرور هفت‌روزه']:
    p('• '+item)
story.append(PageBreak())

title('۱. پایه‌هایی که باید بلد باشید')
section('C# و شی‌گرایی')
p('نوع مقداری مانند int معمولاً خود مقدار را نگه می‌دارد؛ نوع ارجاعی مانند class از طریق ارجاع به شیء دسترسی می‌دهد. struct نوع مقداری است و برای داده‌های کوچک و معمولاً تغییرناپذیر مناسب است. nullable reference types کمک می‌کند احتمال null را آشکار کنید.')
p('کپسوله‌سازی یعنی محافظت از وضعیت داخلی؛ وراثت یعنی ساخت نوع تخصصی از نوع پایه؛ چندریختی یعنی رفتار متفاوت از راه قرارداد مشترک. interface قرارداد قابلیت‌هاست و abstract class می‌تواند هم قرارداد و هم پیاده‌سازی مشترک داشته باشد.')
callout('مثال','IProductService را قرارداد عملیات محصول در نظر بگیرید؛ ProductService پیاده‌سازی آن است. کنترلر به قرارداد وابسته می‌شود و در آزمون می‌توان پیاده‌سازی جایگزین تزریق کرد.')
section('مفاهیم روزمره')
p('• تفاوت List و Dictionary: اولی مجموعهٔ مرتبِ قابل پیمایش است؛ دومی مقدار را با کلید پیدا می‌کند. HashSet اعضای یکتا نگه می‌دارد.\n• LINQ: Where فیلتر، Select تبدیل، OrderBy مرتب‌سازی و FirstOrDefault گرفتن نخستین نتیجه یا مقدار پیش‌فرض است.\n• IEnumerable معمولاً داده را هنگام پیمایش می‌دهد؛ IQueryable امکان ساخت query برای اجرا در منبعی مانند پایگاه داده را فراهم می‌کند.\n• using برای آزادسازی منابعی مانند stream؛ generic برای کد نوع‌امن و قابل استفادهٔ مجدد؛ extension method برای افزودن متد بدون تغییر نوع اصلی.')
section('SOLID و کد تمیز')
p('Single Responsibility: هر کلاس یک دلیل اصلی برای تغییر داشته باشد. Dependency Inversion: وابستگی‌های سطح بالا به abstraction تکیه کنند. برای جونیور کافی است بتوانید مثال ساده بزنید و توضیح دهید این اصل چه تغییری در طراحی ایجاد می‌کند.')
section('نمونه‌متد: پالایش و تبدیل داده با LINQ')
code('public async Task<List<ProductDto>> GetActiveAsync()\n{\n    return await _db.Products\n        .AsNoTracking()\n        .Where(p => p.IsActive)\n        .OrderBy(p => p.Name)\n        .Select(p => new ProductDto(p.Id, p.Name, p.Price))\n        .ToListAsync();\n}')
p('در این مثال شرط و مرتب‌سازی به SQL تبدیل می‌شوند، Select فقط ستون‌های لازم را می‌گیرد و ToListAsync اجرای query را انجام می‌دهد. AsNoTracking برای خواندن بدون ویرایش، هزینهٔ نگهداری وضعیت را کم می‌کند.')
story.append(PageBreak())

title('۲. نمونه‌متدهای کاربردی C#')
section('ساختار سادهٔ DTO و اعتبارسنجی ورودی')
code('public sealed record CreateProductRequest(\n    string Name, decimal Price);\n\npublic static string? Validate(CreateProductRequest request)\n{\n    if (string.IsNullOrWhiteSpace(request.Name))\n        return "Name is required";\n    if (request.Price <= 0)\n        return "Price must be positive";\n    return null;\n}')
p('Record برای مدل‌های داده‌ای مناسب است. در برنامهٔ واقعی اعتبارسنجی را می‌توان با Data Annotations یا کتابخانه/لایهٔ اعتبارسنجی انجام داد؛ پیام خطا باید برای مصرف‌کنندهٔ API روشن باشد.')
section('نمونه‌متد async در سرویس')
code('public async Task<ProductDto?> FindAsync(\n    int id, CancellationToken cancellationToken = default)\n{\n    var product = await _db.Products\n        .AsNoTracking()\n        .Where(p => p.Id == id)\n        .Select(p => new ProductDto(p.Id, p.Name, p.Price))\n        .FirstOrDefaultAsync(cancellationToken);\n\n    return product;\n}')
p('async/await برای I/O مانند پایگاه داده مفید است و thread را هنگام انتظار بی‌دلیل مسدود نمی‌کند. Task<T> نتیجهٔ آینده را نشان می‌دهد. از .Result و .Wait() در مسیر async پرهیز کنید؛ CancellationToken را به عملیات پایین‌دست منتقل کنید.')
section('نمونه‌متد ایجاد محصول با EF Core')
code('public async Task<int> CreateAsync(\n    CreateProductRequest request,\n    CancellationToken cancellationToken = default)\n{\n    var product = new Product\n    {\n        Name = request.Name.Trim(),\n        Price = request.Price,\n        IsActive = true\n    };\n\n    _db.Products.Add(product);\n    await _db.SaveChangesAsync(cancellationToken);\n    return product.Id;\n}')
callout('نکتهٔ مصاحبه','چرا DbContext را Singleton نمی‌کنیم؟ چون برای یک واحد کار طراحی شده، معمولاً scoped است و برای استفادهٔ هم‌زمان thread-safe نیست.')
section('نمونهٔ کنترلر MVC برای دریافت یک محصول')
code('[HttpGet("{id:int}")]\npublic async Task<ActionResult<ProductDto>> GetById(\n    int id, CancellationToken cancellationToken)\n{\n    var product = await _service.FindAsync(id, cancellationToken);\n    if (product is null) return NotFound();\n    return Ok(product);\n}')
p('ActionResult<T> اجازه می‌دهد پاسخ موفقِ دارای داده یا پاسخ HTTP مناسب مانند 404 برگردانید. شناسه در route محدود شده تا ورودی نامعتبر زودتر تشخیص داده شود.')
title('۳. وب، API و Dependency Injection')
section('چرخهٔ درخواست در ASP.NET Core')
p('درخواست از middlewareها عبور می‌کند؛ هر middleware می‌تواند کاری انجام دهد و سپس مرحلهٔ بعد را فراخوانی کند. Routing مقصد را پیدا می‌کند، model binding ورودی را به پارامترها تبدیل می‌کند، validation قواعد را بررسی می‌کند و action پاسخ را می‌سازد.')
section('کدهای وضعیت و HTTP')
p('200 موفق، 201 منبع ساخته شد، 204 موفق بدون بدنه، 400 ورودی نامعتبر، 401 احراز هویت نشده، 403 دسترسی مجاز نیست، 404 پیدا نشد و 500 خطای پیش‌بینی‌نشدهٔ سرور است. GET برای خواندن، POST معمولاً برای ایجاد، PUT برای جایگزینی کامل و PATCH برای تغییر بخشی به‌کار می‌رود.')
callout('تفاوت مهم','401 یعنی درخواست هویت معتبر ندارد؛ 403 یعنی هویت مشخص است ولی مجوز انجام این کار را ندارد.')
section('طول عمر سرویس‌ها')
p('Transient: در هر بار درخواست سرویس جدید ساخته می‌شود. Scoped: یک نمونه در طول درخواست وب. Singleton: یک نمونه برای عمر برنامه. DbContext در برنامه‌های معمول ASP.NET Core به‌صورت scoped ثبت می‌شود.')
code('builder.Services.AddScoped<IProductService, ProductService>();\nbuilder.Services.AddDbContext<AppDbContext>(options =>\n    options.UseSqlServer(connectionString));')
section('احراز هویت و امنیت')
p('Authentication مشخص می‌کند کاربر کیست؛ Authorization تعیین می‌کند چه کاری اجازه دارد. ورودی را اعتبارسنجی کنید، از پارامترهای query یا EF استفاده کنید تا SQL injection رخ ندهد، دادهٔ حساس را در log نگذارید و برای عملیات تغییردهنده از سازوکار ضد CSRF متناسب با نوع برنامه استفاده کنید. رمز عبور را متن ساده ذخیره نکنید.')
section('پرسش کوتاه: middleware چیست؟')
p('جزئی از زنجیرهٔ پردازش درخواست و پاسخ است. نمونه‌ها: ثبت log، مدیریت خطا، احراز هویت و مسیریابی. ترتیب ثبت middleware می‌تواند بر رفتار برنامه اثر بگذارد.')
story.append(PageBreak())

title('۴. EF Core و SQL Server')
section('مفاهیم EF Core')
p('DbContext واحد کار و نقطهٔ تعامل با پایگاه داده است؛ DbSet نمایندهٔ مجموعهٔ یک نوع entity است. Migration تغییرات مدل را به تغییرات schema تبدیل می‌کند. SaveChangesAsync تغییرات ردیابی‌شده را ذخیره می‌کند.')
section('Tracking و AsNoTracking')
p('در query ردیابی‌شده، EF تغییرات entity را دنبال می‌کند تا SaveChanges آن‌ها را بنویسد. برای خواندن صرف، AsNoTracking حافظه و کار اضافی را کم می‌کند. اگر entity را برای تغییر بار می‌کنید، ردیابی معمولاً مناسب است.')
section('JOIN و GROUP BY')
p('INNER JOIN فقط ردیف‌های دارای تطبیق در هر دو طرف را برمی‌گرداند. LEFT JOIN همهٔ ردیف‌های سمت چپ را نگه می‌دارد و در نبود تطبیق، ستون‌های سمت راست NULL هستند. GROUP BY داده را گروه‌بندی می‌کند تا تابع‌هایی مثل COUNT یا SUM روی هر گروه اجرا شوند.')
code('SELECT c.Name, COUNT(o.Id) AS OrderCount\nFROM Customers AS c\nLEFT JOIN Orders AS o ON o.CustomerId = c.Id\nGROUP BY c.Name;')
section('ایندکس و کارایی')
p('ایندکس جستجو و مرتب‌سازی روی ستون‌های مناسب را سریع‌تر می‌کند، اما فضای ذخیره‌سازی می‌گیرد و نوشتن را پرهزینه‌تر می‌کند. query را اندازه بگیرید و برنامهٔ اجرای آن را ببینید؛ همهٔ ستون‌ها را بی‌دلیل ایندکس نکنید.')
section('پرسش‌های عملی')
callout('N+1 چیست؟','وقتی یک query برای فهرست اصلی و سپس یک query جدا برای هر عضو اجرا می‌شود. projection، بارگذاری آگاهانهٔ رابطه‌ها و بررسی تعداد queryها کمک می‌کند.')
callout('Migration امن چطور؟','مدل را تغییر می‌دهم، migration را می‌سازم، SQL تولیدشده را بازبینی می‌کنم، روی محیط آزمایشی می‌سنجم و بعد با پشتیبان‌گیری و روند انتشار تیم اعمال می‌کنم.')
callout('SQL injection؟','ورودی را با string به SQL نچسبانید. از query پارامتری یا EF Core استفاده کنید و مجوزهای دسترسی پایگاه داده را محدود نگه دارید.')
section('فرق PUT و PATCH')
p('PUT معمولاً نمایش کامل منبع را جایگزین می‌کند و در صورت طراحی مناسب idempotent است. PATCH فقط بخشی از منبع را تغییر می‌دهد. قرارداد endpoint باید روشن کند فیلدهای حذف‌شده یا null چه معنایی دارند.')
story.append(PageBreak())

title('۵. پرسش‌های پرتکرار با پاسخ نمونه')
qa=[
('Dependency Injection چیست؟','دادن وابستگی به کلاس از بیرون است؛ وابستگی‌ها را جدا و آزمون‌پذیری را بیشتر می‌کند. مثلاً کنترلر IProductService می‌گیرد و خودش آن را new نمی‌کند.'),
('IEnumerable و IQueryable چه تفاوتی دارند؟','IEnumerable روی دادهٔ در دسترس پیمایش می‌کند. IQueryable عبارت query را می‌سازد تا provider مثل EF آن را به SQL تبدیل کند. اجرای زودهنگام با ToList می‌تواند پردازش را به حافظه منتقل کند.'),
('async/await چه مشکلی را حل می‌کند؟','انتظار برای I/O را بدون مسدودکردن thread جاری مدیریت می‌کند. برای محاسبات CPU-bound به‌تنهایی سرعت ایجاد نمی‌کند.'),
('DTO چرا استفاده می‌شود؟','قرارداد ورودی/خروجی API را از entity پایگاه داده جدا می‌کند، فقط فیلدهای لازم را نشان می‌دهد و احتمال over-posting را کم می‌کند.'),
('Repository و Service چه نقشی دارند؟','Service منطق کاربردی و هماهنگی سناریو را نگه می‌دارد. Repository دسترسی داده را پنهان می‌کند. این لایه‌ها را وقتی اضافه می‌کنم که جداسازی واقعی ایجاد کنند؛ صرفاً برای تعداد کلاس بیشتر نیست.'),
('Unit test چیست؟','آزمونی سریع برای یک رفتار کوچک با وابستگی‌های کنترل‌شده است. منطق قیمت‌گذاری را با ورودی‌های مشخص بررسی می‌کنم؛ رفتار واقعی EF را بهتر است با آزمون یکپارچه سنجید.'),
('باگ را چطور پیدا می‌کنید؟','خطا را بازتولید می‌کنم، ورودی و log را بررسی می‌کنم، محل شکست را با debugger محدود می‌کنم، فرضیه می‌سازم، اصلاح کوچک انجام می‌دهم و یک آزمون برای جلوگیری از برگشت خطا می‌نویسم.'),
('وقتی جواب را نمی‌دانید چه می‌گویید؟','صادقانه می‌گویم تجربهٔ مستقیم ندارم، آنچه می‌دانم را بیان می‌کنم و مسیر بررسی‌ام را توضیح می‌دهم؛ حدس را به‌جای واقعیت مطرح نمی‌کنم.'),
]
for q,a in qa:
    story.append(KeepTogether([rtl_paragraph(q,S['h3fa'],W),
                               rtl_paragraph('پاسخ نمونه: '+a,S['bodyfa'],W)]))
section('پاسخ کوتاه برای معرفی خود')
callout('الگو','من روی C# و ASP.NET Core MVC کار کرده‌ام و با EF Core و SQL Server داده را خوانده و تغییر داده‌ام. در پروژهٔ نمونه، مسیر درخواست را از Controller تا Service و Repository دنبال کرده‌ام. دنبال موقعیتی هستم که با بازخورد تیم، کد قابل‌نگهداری‌تر بنویسم. (این متن را با تجربهٔ واقعی خودتان جایگزین کنید.)')
story.append(PageBreak())

title('۶. تمرین پروژه و برنامهٔ مرور')
section('تمرین: API سادهٔ محصولات')
p('یک API برای Product طراحی کنید: دریافت فهرست فعال، دریافت با شناسه، ایجاد، ویرایش و حذف. برای هر endpoint پاسخ موفق و خطای مناسب را مشخص کنید؛ DTO جدا داشته باشید؛ ورودی نام و قیمت را اعتبارسنجی کنید؛ عملیات پایگاه داده را async بنویسید؛ برای فهرست خواندنی AsNoTracking را در نظر بگیرید.')
section('آنچه در مصاحبه توضیح دهید')
for item in ['چرا controller منطق پایگاه داده را مستقیم اجرا نمی‌کند؟','چرا API نباید entity را بی‌واسطه بپذیرد یا برگرداند؟','در ورودی اشتباه چه status code می‌دهید؟','اگر محصول پیدا نشود چه پاسخی می‌دهید؟','چطور از اجرای query اضافی یا بازگرداندن ستون‌های زیاد جلوگیری می‌کنید؟','چه آزمونی برای قواعد قیمت و نام می‌نویسید؟']:
    p('• '+item)
section('برنامهٔ مرور هفت‌روزه')
rows=[[rtl_paragraph('روز',S['smallfa'],22*mm),rtl_paragraph('تمرکز و خروجی روز',S['smallfa'],W-22*mm)]]
days=[('۱','C#، نوع‌ها، null و OOP؛ برای هر مفهوم یک مثال بگویید.'),('۲','interface، SOLID، collections و LINQ؛ یک فهرست را فیلتر و تبدیل کنید.'),('۳','async/await، exception و cancellation؛ یک متد I/O را توضیح دهید.'),('۴','HTTP، MVC، routing، DI و validation؛ مسیر درخواست را رسم کنید.'),('۵','EF Core، tracking، migration و SQL؛ یک JOIN ساده بنویسید.'),('۶','یک CRUD کوچک را اجرا و با صدای بلند توضیح دهید.'),('۷','معرفی خود، پاسخ رفتاری و پرسش‌های مصاحبه را تمرین کنید.')]
for day,task in days:
    rows.append([rtl_paragraph('روز '+day,S['bodyfa'],22*mm),rtl_paragraph(task,S['bodyfa'],W-22*mm)])
t=Table(rows,colWidths=[22*mm,W-22*mm],repeatRows=1)
t.setStyle(TableStyle([('BACKGROUND',(0,0),(-1,0),PALE),('GRID',(0,0),(-1,-1),.4,LINE),
                       ('VALIGN',(0,0),(-1,-1),'MIDDLE'),('LEFTPADDING',(0,0),(-1,-1),7),
                       ('RIGHTPADDING',(0,0),(-1,-1),7),('TOPPADDING',(0,0),(-1,-1),6),
                       ('BOTTOMPADDING',(0,0),(-1,-1),6)]))
story.extend([t,Spacer(1,8)])
section('چک‌لیست پیش از مصاحبه')
p('□ یک پروژهٔ واقعی یا نمونه را از درخواست تا پایگاه داده شرح بدهم.  □ تفاوت 401 و 403، PUT و PATCH، Tracking و AsNoTracking را بگویم.  □ یک متد async و یک query LINQ را بدون حفظ‌کردن توضیح بدهم.  □ برای پرسیدن سؤال از مصاحبه‌گر آماده باشم: تیم review کد را چطور انجام می‌دهد؟')
callout('یادآوری','پاسخ خوب معمولاً شامل تعریف کوتاه، دلیل استفاده، یک مثال و یک محدودیت است. اگر بخشی از کد را ننوشته‌اید، آن را تجربهٔ خودتان معرفی نکنید.')

doc.build(story,onFirstPage=header_footer,onLaterPages=header_footer)
print(str(OUT).encode('ascii','backslashreplace').decode('ascii'))
