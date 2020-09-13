

var addCompanyWinForm = new Ext.form.FormPanel({
    buttonAlign: 'center',
    frame: true,
    region: "center",
    url: '/CompanyPush/AddCompany',
    items: [{
        layout: 'form',
        xtype: 'fieldset',
        style: 'margin-left:10px;margin-right:10px',
        autoHeight: true,
        title: '基本信息',
        labelAlign: 'right',
        items: [{
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            
            items: [{
                xtype: "areacombobox",
                columnWidth: .50,
                fieldLabel: '区域',
                labelAlign: 'right',
                name: 'AreaId'
            },
            {
                columnWidth: .50,
                fieldLabel: '企业名称',
                name: "CompanyName",
                labelAlign: 'right'
            }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [{
                columnWidth: .50,
                fieldLabel: '法人',
                name: "ArtificialPerson",
                labelAlign: 'right'
            },
            {
                xtype: "datefield",
                format: 'Y-m-d',
                columnWidth: .50,
                fieldLabel: '成立日期',
                name: "SetUpTime",
                labelAlign: 'right'
            }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [{
                columnWidth: .50,
                fieldLabel: '联系电话',
                name: "CompanyPhone",
                labelAlign: 'right'
            },
            {
                columnWidth: .50,
                fieldLabel: '住所1',
                name: "Address",
                labelAlign: 'right'
            }]
        }]
    }, {
        layout: 'form',
        xtype: 'fieldset',
        style: 'margin-left:10px;margin-right:10px',
        autoHeight: true,
        title: '信息维护',
        labelAlign: 'right',
        items: [{
            layout: "column",
            frame: true,
            style: 'border:0',
            defaultType: 'textfield',
            items: [{
                columnWidth: .50,
                xtype: "fieldcontainer",
                items: [{
                    width: 300,
                    xtype: "datefield",
                    format: 'Y-m-d',
                    fieldLabel: '电销联系日期',
                    name: "PhoneConnectDate",
                    labelAlign: 'right'
                }]
            },
            {
                columnWidth: .50,
                xtype: "fieldcontainer",
                items: [{
                    xtype: 'combo',
                    fieldLabel: '公司现状',
                    labelAlign: 'right',
                    name: 'CompanyState',
                    store: new Ext.data.SimpleStore({
                        fields: ['value', 'text'],
                        data: [
                            ['1', '已注销'],
                            ['2', '存续'],
                            ['3', '待确认'],
                        ]
                    }),
                    displayField: 'text',
                    valueField: 'value',
                    mode: 'local',
                    emptyText: '请选择',
                    readOnly: false
                }]
            }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [
                {
                    columnWidth: 1,
                    xtype: "fieldcontainer",
                    layout: 'hbox',
                    items: [{
                        xtype: 'combo',
                        width: 300,
                        fieldLabel: '是否有效地址',
                        labelAlign: 'right',
                        name: 'IsValidateAddress',
                        store: new Ext.data.SimpleStore({
                            fields: ['value', 'text'],
                            data: [
                                ['1', '有效'],
                                ['2', '无效']
                            ]
                        }),
                        displayField: 'text',
                        valueField: 'value',
                        mode: 'local',
                        emptyText: '请选择',
                        readOnly: false,
                        listeners: {
                            select: function (combo, res, index) {
                                var value = res[0].get("value");
                                if (value == "2") {
                                    addCompanyWinForm.getForm().findField("ConfirmAddress").show();
                                }
                                else {
                                    addCompanyWinForm.getForm().findField("ConfirmAddress").setValue("");
                                    addCompanyWinForm.getForm().findField("ConfirmAddress").hide();
                                }
                            }
                        }
                    },
                    {
                        xtype: "textfield",
                        width: "80%",
                        fieldLabel: '确认地址',
                        name: "ConfirmAddress",
                        labelAlign: 'right',
                        hidden: true
                    }]

                }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [
                {
                    columnWidth: .50,
                    fieldLabel: '上门拜访人员',
                    name: "GoHomePerson",
                    labelAlign: 'right'
                }, {
                    columnWidth: .50,
                    xtype: "fieldcontainer",
                    items: [{
                        xtype: "datefield",
                        format: 'Y-m-d',
                        fieldLabel: '拜访时间',
                        name: "GoHomeTime",
                        labelAlign: 'right'
                    }]
                }]
        }, {
            layout: "column",
            style: 'border:0',
            frame: true,
            defaultType: 'textfield',
            items: [{
                xtype: 'textarea',
                cols: 7,
                columnWidth: .50,
                fieldLabel: '电话联系情况',
                name: "PhoneConnectState",
                labelAlign: 'right'
            }, {
                    columnWidth: .50,
                    xtype: "fieldcontainer",
                    items: [{
                        xtype: 'combo',
                        fieldLabel: '合作意向情况',
                        labelAlign: 'right',
                        name: 'CooperationIntention',
                        store: new Ext.data.SimpleStore({
                            fields: ['value', 'text'],
                            data: [
                                ['1', '无意向'],
                                ['2', '有意向'],
                                ['3', '待确认'],
                            ]
                        }),
                        displayField: 'text',
                        valueField: 'value',
                        mode: 'local',
                        emptyText: '请选择',
                        readOnly: false
                    }]
                }]
        }]
    }]
});


// 反查数据window
var addCompanyWin = Ext.create("Ext.window.Window",
    {
        title: '新增公司',
        modal: true,
        constrainHeader: true,
        buttonAlign: 'center',
        resizable: false,
        height: 450,
        width: 1000,
        layout: "fit",
        items: [addCompanyWinForm],
        closeAction: 'hide',
        buttons: [{
            xtype: "button",
            width: 50,
            text: "保存",
            handler: function () {
                var form = addCompanyWinForm.getForm();
                if (form.isValid()) {
                    form.submit({
                        success: function (form, action) {
                            Ext.Msg.alert('提示信息', "添加成功");
                            addCompanyWin.hide();
                            areaTree.getStore().load();
                        },
                        failure: function (form, action) {
                            Ext.Msg.alert('提示信息', action.result.restMsg);
                        }
                    });
                }
            }
        }],
        listeners: {
            show: function(){
                addCompanyWinForm.getForm().reset();
            }
        }
    });