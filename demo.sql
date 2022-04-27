create table student (id int primary key,stname varchar(20),gender varchar(20));
insert into student values(1,"关羽","男");
insert into student values(2,"刘备","男");
insert into student values(3,"貂蝉","女");

 create table orderbill (billno varchar(20) primary key,billdate datetime,customer varchar(50),customeraddress varchar(20));
insert into orderbill values("20210818001","2021-08-18","株洲健坤","九郎山家园");
insert into orderbill values("20210818002","2021-08-18","中房电脑城","芦淞区解放街100号");
