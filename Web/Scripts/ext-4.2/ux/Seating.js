/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:4/2/2013 5:07:57 PM
Description:座次列席分布组件
*************************************************************************************************/
Ext.define('BeidaSoft.chart.Seating', {
    extend: 'Ext.panel.Panel', //为避免不兼容旧版本ie，这里直接继承于Panel而不是Ext.draw.Component
    alias: 'widget.bdseating',
    layout: 'fit',
    viewBox: false,
    border: false,
    drawCmp: null, //绘图画板，私有变量
    scene:{width:400,height:600,top:1,right:10,paddingTop:200,paddingLeft:200 },//场景配置,top、right等代表每个面座位数（默认left=right）
    seat:{width:60,height:70,photo:{width:100,height:120}},//每个席位配置
    person: null, //列席人员信息
    debug: false, //是否为调试模式
    constructor: function (config) {
        if (config) {
            Ext.apply(this, config);
        }
        this.callParent(arguments);
    },

    initComponent: function () {
        var me = this;
        this.addEvents('mapmouseover', 'mapmouseout', 'mapmousedown', 'mapmouseup'); //定义鼠标移入、移出事件

        //定义此作用域内全局变量
        this.defaultConfig = {//默认进度配置
            backgroundColor: { 0: { color: '#EEEEEE' }, 50: { color: '#E5E5E5' }, 80: { color: '#DBDBDB' }, 100: { color: '#EDEDED'} }
        };

        //绘图对象初始化
        me.drawCmp = Ext.create('Ext.draw.Component', {
            gradients: [
                {
                    id: 'backgroundGradient',
                    angle: 90,
                    stops: me.defaultConfig.backgroundColor
                }
            ]
        });
        me.items = me.drawCmp;

        this.callParent(arguments);
    },
    //onRender:function(){
    //    this.callParent(arguments);
    //},
    afterRender: function () {//由于各个精灵使用surface添加，而surface必须渲染后才能使用，因此添加精灵操作放到afterRender中
        var me = this;
        this.callParent(arguments);
        //添加进度事件
        this.drawCmp.surface.on('mousedown', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmousedown', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });
        this.drawCmp.surface.on('mouseup', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmouseup', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });
        this.drawCmp.surface.on('mouseover', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmouseover', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });
        this.drawCmp.surface.on('mouseout', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmouseout', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });

        //程序主要流程
        me.drawBackground();
        for(var i=0,len=me.persons.length;i<len;i+=1){
            me.drawSeat(me.persons[i]);
        }
    },
    onDestroy: function () {
        delete this.defaultConfig;
    },
    getSpriteCenterCoordinate: function (sprite) {//私有方法，取得区域中心坐标
        var bbox = sprite.getBBox();
        return { x: bbox.x + bbox.width / 2, y: bbox.y + bbox.height / 2 };
    },
    progressMouseover: function (area, toColor, fromColor, spriteComposite, e, t, eOpts) {//私有方法，鼠标移入动画
        var me = this;

    },
    progressMouseout: function (area, toColor, fromColor, spriteComposite, e, t, eOpts) {//私有方法，鼠标移出动画
        var me = this;

    },
    progressMouseup: function (area, toColor, fromColor, spriteComposite, e) {
        var me = this;

    },
    getSeatPosition:function(person){//私有方法，取得每个席位所在位置,person的索引从0开始,暂时只考虑top为1的情况
        var x=0;
        var multiple=Math.ceil(person.index/2);
        if(person.index===0){
            x=this.scene.paddingLeft+this.scene.width/2-this.seat.width/2;
        }else if(person.index%2===1){
            x=this.scene.paddingLeft-this.seat.width-20;//20是间隔
        }else{
            x=this.scene.paddingLeft+this.scene.width+20;//20是间隔
        }
        var y=this.scene.paddingTop/2+multiple*this.seat.height*1.5;//1.5中的0.5代表间隔
        return {x:x,y:y};
    },
    drawBackground: function () {
//        var bg = Ext.create('Ext.draw.Sprite', {
//            type: 'rect',
//            width: this.scene.width,
//            height: this.scene.height,
//            radius: 5,
//            fill: 'url(#backgroundGradient)',
//            opacity: 1,
//            stroke: '#B3B3B3',
//            'stroke-width': 1,
//            x:this.scene.paddingLeft,
//            y:this.scene.paddingTop
        //        });
        var bg = Ext.create('Ext.draw.Sprite', {
            type: "image",
            src: '/modules/commons/js/ext-4.2/ux/images/desktopPaper.png',
            width: this.scene.width,
            height: this.scene.height,
            x:this.scene.paddingLeft,
            y:this.scene.paddingTop
        });
        this.drawCmp.surface.add(bg);
        bg.show(true);
        return bg;
    },
    drawSeat:function(person){//单个人席位绘制
        var spriteGroup = Ext.create('Ext.draw.CompositeSprite');
        var photoSrpite=Ext.create('Ext.draw.Sprite',{
            type: "image",
            src: person.url,
            width: this.seat.width,
            height: this.seat.height,
            x:this.getSeatPosition(person).x,
            y:this.getSeatPosition(person).y
        });
        var nameSprite = Ext.create('Ext.draw.Sprite', {
            type: 'text',
            text: person.name,
            x:this.getSeatPosition(person).x+12,
            y:this.getSeatPosition(person).y+this.seat.height+20, //20是姓名与照片的间隔
            font:'10pt 微软雅黑'
        });
        this.drawCmp.surface.add(photoSrpite);
        this.drawCmp.surface.add(nameSprite);
        spriteGroup.add(photoSrpite);
        spriteGroup.add(nameSprite);
        spriteGroup.show(true);
        return spriteGroup;
    }
});