DELIMITER $$
USE `FinancialDataAnalysis`$$

DROP PROCEDURE IF EXISTS EARS_EARSMain_SP_Temp_DML$$
CREATE PROCEDURE EARS_EARSMain_SP_Temp_DML()
BEGIN
	IF NOT EXISTS (select 1 from information_schema.tables where table_schema='FinancialDataAnalysis' and table_name='AccountItem'  )
	THEN
		create table AccountItem
		(
		   ExcelRecordId        int comment '对应的excel',
		   AccountCode          varchar(30) comment '科目编号',
		   ParentCode           varchar(30) comment '父级编号',
		   AccountName          varchar(30) comment '科目名称',
		   AccountLevel         tinyint comment '科目层级',
		   AccountTypeId       tinyint comment '类型：1：收入，2：支出',
		   AccountSumTypeId     tinyint comment '相加类型：1：+  2：-',
		   IsLeaf               tinyint comment '是否叶子',
           index IX_AccountItem_ExcelRecordId(ExcelRecordId)
		) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COMMENT='科目';
        
        SET SQL_SAFE_UPDATES = 0;
		delete from accountitem;
		insert into accountitem
		values(1,'6001','0','主营业务收入',1,1,1,0),
		(1,'600101','6001','物业服务',2,1,1,0),
		(1,'60010101','600101','物业服务费收入',3,1,1,1),
		(1,'60010102','600101','车位收入',3,1,1,1),
		(1,'6051','0','其他业务收入',1,1,1,0),
		(1,'605101','6051','工本费收入',2,1,1,1),
		(1,'605102','6051','多种经营收入',2,1,1,0),
		(1,'60510201','605102','电梯广告收入',3,1,1,1);
	end if;
    
    IF NOT EXISTS (select 1 from information_schema.tables where table_schema='FinancialDataAnalysis' and table_name='ExcelRecord'  )
	THEN
		create table ExcelRecord
		(
		   ExcelRecordId        int not null auto_increment comment '主键',
		   ExcelName            varchar(200) comment 'excel名称',
		   CreateDateTime       datetime comment '创建日期',
		   ExcelUrl             varchar(500) comment 'excel地址',
		   StatusFlag           tinyint comment '状态',
		   primary key (ExcelRecordId)
		) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COMMENT='excel导入记录';
	end if;
    
    IF NOT EXISTS (select 1 from information_schema.tables where table_schema='FinancialDataAnalysis' and table_name='FinancialDataItem'  )
	THEN
		create table FinancialDataItem
		(
		   ExcelRecordId        int comment '对应的excel',
		   ItemId               int comment '项目Id',
		   ParentId             int comment '父级Id',
           XingZhiId			int comment '所属性质，只有项目层级有',
		   ItemName             varchar(100) comment '项目名称',
		   ItemLevel            int comment '项目层级',
		   IsLeaf               int comment '是否叶子',
		   ItemTypeId           int comment '类型：1：虚拟集团 2：事业部，3：片区，4：性质，5：项目',
           index IX_FinancialDataItem_ExcelRecordId(ExcelRecordId)
		) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COMMENT='项目集合';
        
        SET SQL_SAFE_UPDATES = 0;
		delete from financialdataitem;
		insert into financialdataitem
		values(1,1,0,'住宅产业集团',1,0,1),
		(1,2,1,'职能',2,0,1),
		(1,3,2,'梁军片区(旧)',3,1,1),
		(1,6001,0,'新商业产业集团',1,1,1);
	end if;

	IF NOT EXISTS (select 1 from information_schema.tables where table_schema='FinancialDataAnalysis' and table_name='FinancialData'  )
	THEN
		create table FinancialData
		(
		   FinancialDataId      int not null auto_increment,
		   ExcelRecordId        int comment '所属excel',
		   XiangMuIdId          int comment '项目Id',
		   XingZhiId            int comment '性质',
		   PianQuId             int comment '片区',
		   ShiYeBuId            int comment '事业部',
		   AccountCode          int comment '科目Id',
		   QiJianTypeId         int comment '会计期间：1：本年，2：本月',
		   Data                 double comment '数据',
		   primary key (FinancialDataId)
		) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COMMENT='数据集合';
	end if;
    
END$$

CALL EARS_EARSMain_SP_Temp_DML()$$
DROP PROCEDURE IF EXISTS EARS_EARSMain_SP_Temp_DML$$


DROP PROCEDURE IF EXISTS AccountItem_GetByExcelRecordId$$
CREATE PROCEDURE `AccountItem_GetByExcelRecordId`(
	v_ExcelRecordId int
)
    SQL SECURITY INVOKER
BEGIN
/* 
创建者:		高峰
创建日期:	2019-08-16
描述：		获取excelid下所有科目	
修改记录:
--------------------------------------------------------
修改者		修改日期		修改目的

--------------------------------------------------------
*/
	SELECT 
		*
	FROM `AccountItem`
    where ExcelRecordId = v_ExcelRecordId;
END$$

DROP PROCEDURE IF EXISTS ExcelRecord_Get$$
CREATE PROCEDURE `ExcelRecord_Get`(
)
    SQL SECURITY INVOKER
BEGIN
/* 
创建者:		高峰
创建日期:	2019-08-16
描述：		获取所有excel记录	
修改记录:
--------------------------------------------------------
修改者		修改日期		修改目的

--------------------------------------------------------
*/
	SELECT 
		*
	FROM `ExcelRecord`;
END$$


DROP PROCEDURE IF EXISTS `Excelrecord_Save`$$
CREATE PROCEDURE `Excelrecord_Save`(
	v_ExcelRecordId int(11)
	,v_ExcelName varchar(200)
	,v_CreateDateTime datetime
	,v_ExcelUrl varchar(500)
	,v_StatusFlag tinyint(4)
)
	SQL SECURITY INVOKER
BEGIN
/* 
创建者:		高峰
创建日期:	2019-08-16
描述：		保存Excelrecord
修改记录:
--------------------------------------------------------
修改者		修改日期		修改目的
高峰		2019-08-16		创建
--------------------------------------------------------
*/
	IF NOT EXISTS (SELECT 1 FROM `Excelrecord` WHERE `ExcelRecordId`=v_ExcelRecordId) THEN
		INSERT INTO `Excelrecord`(
			`ExcelName`,
			`CreateDateTime`,
			`ExcelUrl`,
			`StatusFlag`
		) VALUES(
			v_ExcelName,
			v_CreateDateTime,
			v_ExcelUrl,
			v_StatusFlag
		);
		SELECT LAST_INSERT_ID();
	ELSE 
		UPDATE `Excelrecord` SET
			`ExcelName`=IFNULL(v_ExcelName,`ExcelName`),
			`CreateDateTime`=IFNULL(v_CreateDateTime,`CreateDateTime`),
			`ExcelUrl`=IFNULL(v_ExcelUrl,`ExcelUrl`),
			`StatusFlag`=IFNULL(v_StatusFlag,`StatusFlag`)
		WHERE 
			`ExcelRecordId`=v_ExcelRecordId
		;
		SELECT v_ExcelRecordId;
	END IF;
END$$

DROP PROCEDURE IF EXISTS FinancialDataItem_GetByExcelRecordId$$
CREATE PROCEDURE `FinancialDataItem_GetByExcelRecordId`(
	v_ExcelRecordId int
)
    SQL SECURITY INVOKER
BEGIN
/* 
创建者:		高峰
创建日期:	2019-08-16
描述：		获取所有excel记录	
修改记录:
--------------------------------------------------------
修改者		修改日期		修改目的

--------------------------------------------------------
*/
	SELECT 
		*
	FROM `FinancialDataItem`
    where ExcelRecordId = v_ExcelRecordId;
END$$

DROP PROCEDURE IF EXISTS FinancialData_GetByFilter$$
CREATE PROCEDURE `FinancialData_GetByFilter`(
	v_ExcelRecordId int(11)
	,v_XiangMuIdId int(11)
	,v_XingZhiId int(11)
	,v_PianQuId int(11)
	,v_ShiYeBuId int(11)
	,v_AccountCode int(11)
	,v_QiJianTypeId int(11)
)
    SQL SECURITY INVOKER
BEGIN
/* 
创建者:		高峰
创建日期:	2019-08-16
描述：		获取所有excel记录	
修改记录:
--------------------------------------------------------
修改者		修改日期		修改目的

--------------------------------------------------------
*/
	SELECT 
		*
	FROM `FinancialData`
    where ExcelRecordId = v_ExcelRecordId
    and (v_XiangMuIdId = 0 or XiangMuIdId=v_XiangMuIdId)
    and (v_XingZhiId = 0 or XingZhiId=v_XingZhiId)
    and (v_PianQuId = 0 or PianQuId=v_PianQuId)
    and (v_ShiYeBuId = 0 or ShiYeBuId=v_ShiYeBuId)
    and (v_AccountCode is null or AccountCode=v_AccountCode)
    and (v_QiJianTypeId = 0 or QiJianTypeId=v_QiJianTypeId)
    ;
END$$

DROP PROCEDURE IF EXISTS FinancialData_GetDetailByPaging$$
CREATE PROCEDURE `FinancialData_GetDetailByPaging`(
	v_ExcelRecordId int,
	v_AccountCode varchar(50),
	v_QiJianTypeId tinyint,
	v_XiangMuTypeId tinyint,
	v_XiangMuItemId varchar(4000),
	v_PageIndex	INT,
	v_PageSize	INT,
	OUT v_TotalCount INT
)
    SQL SECURITY INVOKER
BEGIN

set @sqlcount=' select count(d.FinancialDataId) INTO @TotalCount ';
set @sqlcol=concat(' SELECT 
						d.FinancialDataId as financialdataid,
						a.AccountName as accountname, 
						shiyebu.ItemName as shiyebu,
						pianqu.ItemName as pianqu,
						xingzhi.ItemName as xingzhi,
						xiangmu.ItemName as xiangmu,
						d.data as detaildata ');
set @fromStr=' FROM financialdataanalysis.financialdata d
			left join financialdataitem shiyebu on d.excelrecordid=shiyebu.excelrecordid and d.ShiYeBuId = shiyebu.ItemId
			left join financialdataitem pianqu on d.excelrecordid=pianqu.excelrecordid and d.PianQuId = pianqu.ItemId
			left join financialdataitem xingzhi on d.excelrecordid=xingzhi.excelrecordid and d.PianQuId = xingzhi.ItemId
			left join financialdataitem xiangmu on d.excelrecordid=xiangmu.excelrecordid and d.PianQuId = xiangmu.ItemId
			left join accountitem a on d.excelrecordid = a.excelrecordid and a.AccountCode = d.AccountCode ';
set @whereStr = concat('where d.excelrecordid = ',v_ExcelRecordId,' and d.AccountCode="',v_AccountCode,'" and d.QiJianTypeId=',v_QiJianTypeId);

if v_XiangMuTypeId = 1
	then set @whereStr=concat(@whereStr,' and d.ShiYeBuId in (',v_XiangMuItemId,')' );
elseif v_XiangMuTypeId = 2
	then set @whereStr=concat(@whereStr,' and d.ShiYeBuId = ',v_XiangMuItemId );
elseif v_XiangMuTypeId = 3
	then set @whereStr=concat(@whereStr,' and d.PianQuId = ',v_XiangMuItemId );
elseif v_XiangMuTypeId = 4
	then set @whereStr=concat(@whereStr,' and d.XingZhiId = ',v_XiangMuItemId );
elseif v_XiangMuTypeId = 5
	then set @whereStr=concat(@whereStr,' and d.XiangMuIdId = ',v_XiangMuItemId );
end if;

set @sqlOrder=concat(' ORDER BY d.ShiYeBuId,d.PianQuId,d.XingZhiId,d.XiangMuIdId LIMIT ',v_PageIndex,',',v_PageSize);

set @sqlcount=concat(@sqlcount,@fromStr, @whereStr);
PREPARE sqlcommend from @sqlcount;
execute sqlcommend;
select @TotalCount  into v_TotalCount;

set @sqlstr=concat(@fromStr,@fromStr, @whereStr,@sqlOrder);
PREPARE sqlcommend from @sqlstr;
execute sqlcommend;

END$$