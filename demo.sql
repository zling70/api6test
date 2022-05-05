drop table student;
create table student (id int primary key,stname varchar(20),gender varchar(20));
insert into student values(1,"关羽","男");
insert into student values(2,"刘备","男");
insert into student values(3,"貂蝉","女");
--简易案例用表
drop table orderbill;
create table orderbill (billno varchar(50) primary key,billdate datetime,customer varchar(50),customeraddress varchar(20));
insert into orderbill values("20210818001","2021-08-18","株洲健坤","九郎山家园");
insert into orderbill values("20210818002","2021-08-18","中房电脑城","芦淞区解放街100号");
insert into orderbill values("20210818003","2021-08-19","株洲健坤","北京");
insert into orderbill values("20210818004","2021-08-19","中房电脑城","上海");

--复杂单据案例用表
drop table customer;
create table customer(
linenum int ,	-- 行号
customer varchar(50) primary key,	-- 客户编号,这里没有使用customerid之类
custname varchar(50),	-- 客户名称
custaddress varchar(50)	-- 客户地址
);

insert into customer values(1, "c01", "刘小备","长沙" );
insert into customer values(2, "c02", "关小羽","湘潭" );
insert into customer values(3, "c03", "张小飞","株洲" );
insert into customer values(4, "c04", "赵小云","衡阳" );
insert into customer values(5, "c05", "周小瑜","岳阳" );
insert into customer values(6, "c06", "孔小明","邵阳" );


drop table orderbill;
create table orderbill (
    billno varchar(50) primary key,-- 单据编号
    billdate datetime,-- 单据日期
    customer varchar(50),-- 客户（编号）
    customeraddress varchar(20),-- 客户地址
    billstatus varchar(10),-- 单据审核状况
    remark varchar(200),-- 备注
    currid varchar(50), -- 币别
    billstylename varchar(50),-- 单据类型
    maker varchar(50),-- 制单人员
    permitter varchar(50),-- 审核人员
    priceofoftax varchar(20),-- 单价是否含税
    salesid varchar(50),-- 业务员
    departid varchar(50) -- 部门(编号)
    );
drop table orderitems;
create table orderitems(
    linenum int,-- 行号
    billno varchar(50),-- 单据编号
    code varchar(50),-- 物料编号
    name varchar(100),-- 物料名称
    unitof varchar(20),-- 计量单位
    unitprice decimal(14,4),-- 单价
    quantities decimal(14,4),-- 数量
    amounts decimal(14,4),-- 总金额
    specs varchar(50),-- 规格型号
    primary key(linenum,billno)
 );
alter table orderbill add constraint fk_2customer foreign key (customer) references customer (customer);
alter table orderitems add constraint fk_2orderbill foreign key (billno) references orderbill (billno);
===========老数据，customer字段填写的是直接地址，不是地址编号
insert into orderbill(billno,billdate,customer,customeraddress,billstatus,remark ,currid,billstylename,maker,permitter,priceofoftax,salesid ,departid) 
values("20210818001","2021-08-18","株洲健坤","九郎山家园","未结案","写入备注:你是一条汉子！","RMB","打折销售","admin","刘经理","是","小帅","后勤部");

insert into orderbill (billno,billdate,customer,customeraddress,billstatus,remark ,currid,billstylename,maker,permitter,priceofoftax,salesid ,departid) 
values("20210818002","2021-08-18","中房电脑城","芦淞区解放街100号","未结案","写入备注:你是一条汉子！","RMB","打折销售","admin","刘经理","是","小猪","办公室");

insert into orderbill (billno,billdate,customer,customeraddress,billstatus,remark ,currid,billstylename,maker,permitter,priceofoftax,salesid ,departid) 
values("20210818003","2021-08-19","株洲健坤","北京","未结案","写入备注:你是一条汉子！","RMB","打折销售","关小羽","王总","否","小李","销售部");

insert into orderbill (billno,billdate,customer,customeraddress,billstatus,remark ,currid,billstylename,maker,permitter,priceofoftax,salesid ,departid) 
values("20210818004","2021-08-19","中房电脑城","上海","未结案","写入备注:你是一条汉子！","RMB","打折销售","关小羽","李总","否","小花","销售部");
=============新数据，填写的是客户编号=============
insert into orderbill(billno,billdate,customer,customeraddress,billstatus,remark ,currid,billstylename,maker,permitter,priceofoftax,salesid ,departid) 
values("20210818001","2021-08-18","c01","九郎山家园","未结案","写入备注:你是一条汉子！","RMB","打折销售","admin","刘经理","是","小帅","后勤部");

insert into orderbill (billno,billdate,customer,customeraddress,billstatus,remark ,currid,billstylename,maker,permitter,priceofoftax,salesid ,departid) 
values("20210818002","2021-08-18","c02","芦淞区解放街100号","未结案","写入备注:你是一条汉子！","RMB","打折销售","admin","刘经理","是","小猪","办公室");

insert into orderbill (billno,billdate,customer,customeraddress,billstatus,remark ,currid,billstylename,maker,permitter,priceofoftax,salesid ,departid) 
values("20210818003","2021-08-19","c02","北京","未结案","写入备注:你是一条汉子！","RMB","打折销售","关小羽","王总","否","小李","销售部");

insert into orderbill (billno,billdate,customer,customeraddress,billstatus,remark ,currid,billstylename,maker,permitter,priceofoftax,salesid ,departid) 
values("20210818004","2021-08-19","c03","上海","未结案","写入备注:你是一条汉子！","RMB","打折销售","关小羽","李总","否","小花","销售部");

insert into orderbill (billno,billdate,customer,customeraddress,billstatus,remark ,currid,billstylename,maker,permitter,priceofoftax,salesid ,departid) 
values("20210818114","2021-08-19","c08","上海","未结案","写入备注:你是一条汉子！","RMB","打折销售","关小羽","李总","否","小花","销售部");

-- 使用api6test项目运行后，生成的查询语句
      SELECT TOP(1) [o].[billno], [o].[billdate], [o].[billstatus], [o].[billstylename], [o].[currid], [o].[customer], [o].[customeraddress], [o].[departid], [o].[maker], [o].[permitter], [o].[priceofoftax], [o].[remark], [o].[salesid]
      FROM [Orderbill] AS [o]
      WHERE [o].[billno] = "20210818114"


--明细表数据
insert into orderitems(linenum,billno,code,name,unitof,unitprice,quantities,amounts,specs)
values(1,'20210818001','FI-SW-01','笔记本','本',11200,3,33600,'a01');
insert into orderitems(linenum,billno,code,name,unitof,unitprice,quantities,amounts,specs)
values(2,'20210818001','FI-SW-01','笔记本','本',9600,10,96000,'ao1');
insert into orderitems(linenum,billno,code,name,unitof,unitprice,quantities,amounts,specs)
values(3,'20210818001','FI-SW-01','笔记本','本',7800,1,7800,'a02');
insert into orderitems(linenum,billno,code,name,unitof,unitprice,quantities,amounts,specs)
values(1,'20210818002','K9-DL-01','手机','台',3399,10,33990,'sb01');
insert into orderitems(linenum,billno,code,name,unitof,unitprice,quantities,amounts,specs)
values(2,'20210818002','K9-DL-01','手机','台',2001,2,4002,'sb02');
insert into orderitems(linenum,billno,code,name,unitof,unitprice,quantities,amounts,specs)
values(1,'20210818003','RP-SN-01','手表','支',996,1,996,'ssma');
insert into orderitems(linenum,billno,code,name,unitof,unitprice,quantities,amounts,specs)
values(2,'20210818003','RP-SN-01','手表','支',1002,2,2004,'ssmb');
insert into orderitems(linenum,billno,code,name,unitof,unitprice,quantities,amounts,specs)
values(1,'20210818004','FL-DSH-01','键盘','个',45.5,10,455,'syb');
insert into orderitems(linenum,billno,code,name,unitof,unitprice,quantities,amounts,specs)
values(2,'20210818004','FL-DSH-01','键盘','个',50,2,100,'sxl');