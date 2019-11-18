import React from 'react';
import PropTypes from 'prop-types';
import InputLabel from "@material-ui/core/InputLabel";
import MenuItem from "@material-ui/core/MenuItem";
import FormControl from "@material-ui/core/FormControl";
import TransportProblem from "../pages/TransportProblem";

const Select = ({className,value,label,name,options,onChange}) => {
    return (
        <div>
        <label htmlFor={name}>{label}</label>
        <select name={name} value={value} onChange={onChange} className={className} >
            {options.map(option=><option value={option.value} key={option.value}>{option.label}</option>)}
        </select>
        </div>
    );
};
Select.propTypes = {
    options:PropTypes.array
};
Select.defaultProps = {
    options:[]
};

export default Select;
